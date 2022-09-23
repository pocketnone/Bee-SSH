const express = require('express');
const router = express.Router();
const bcrypt = require('bcryptjs');
const randomstring = require("randomstring");
const mfa = require('../config/totpAuth');
const rateLimit = require('express-rate-limit');

// Databases
const User = require('../models/User');
const sshdb = require('../models/SSHSessions');
const AuthCookie = require('../models/AuthCookie');
const UserScripteDB = require('../models/CustomUserScripts');

var limiter = rateLimit({
    windowMs: 2 * 60 * 1000, // 2 minutes
    max: 20, // Limit each IP to 20 requests per `window` (here, per 2 minutes)
    standardHeaders: true, // Return rate limit info in the `RateLimit-*` headers
    keyGenerator: (request, response) => request.cf_ip, // Set the Cloudflare IP
    legacyHeaders: false, // Disable the `X-RateLimit-*` headers
});

// Ratelimit the API
router.use(limiter);

// API
// Get App Version
router.get("/client_version", (req, res) =>{
    res.json({
        version: process.env.CLIENTVERSION
    }).status(200);
});


// REST Login
router.post("/client_login", (req, res) => {
    const {tool, email, password, otp } = req.body;
    const IPAdress = req.cf_ip;
    // Errors
    let errors = [];

    // Start check
    if(tool != process.env.CLIENTPASSWORD) {
        errors.push({msg: "No ClientPassword"});
    }
    if(!email) {
        errors.push({msg: "No Email"});
    }
    if(!password) {
        errors.push({msg: "no password"});
    }
    if(errors > 0){
        return res.json({Info: errors, Data: req.body, Errors: errors.length}).status(400);
    }

    User.findOne({email: email}).then(user => {
       if(!user)
           return res.json({Info: "No User"}).status(400);

        bcrypt.compare(password + process.env.PASSPEPPER, user.password, (err, isMatch) => {
            console.log(user.name);
            const _username = user.name;
            if (err) throw err;
            if (isMatch) {
                if(user.mfa) {
                    if(!otp)
                        return res.json({Info: "2FA Error"}).status(201);
                    if(mfa(user.secret, otp)) {
                        sshdb.find({UID: user.UID}).then(sshdata => {
                            console.log(sshdata);
                            AuthCookie.findOneAndDelete({UID: user.UID}).then(()=>{});
                            const authcookie = randomstring.generate(55);
                            const new_authcookie = new AuthCookie({
                                UID: user.UID,
                                AuthCookie: authcookie,
                                IP: IPAdress
                            }).save();
                            return res.json({
                                Version: "1.ß",
                                AuthKey: authcookie,
                                Username: _username,
                                data: sshdata
                            }).status(200);
                        })
                    } else {
                        return res.json({Info: "2FA Error"}).status(201);
                    }
                } else {
                    // if there no 2FA
                    AuthCookie.findOneAndDelete({UID: user.UID}).then(()=>{});
                    sshdb.find({UID: user.UID}).then(sshdata => {
                        const authcookie = randomstring.generate(55);
                        const new_authcookie = new AuthCookie({
                            UID: user.UID,
                            AuthCookie: authcookie,
                            IP: IPAdress
                        }).save();
                        return res.json({
                            Version: "1.ß",
                            AuthKey: authcookie,
                            Username: _username,
                            data: sshdata
                        }).status(200);
                    })
                }
            } else {
                return res.json({Info: "Invalid Password"}).status(400);
            }
        });
    });
});

//region Userser Shortcuts

// User Scripte

router.post("/fetch_userscripte", (req, res) => {
    const { authkey, tool } = req.body;
    if(!authkey) {
        return res.status(404);
    }
    if(!tool) {
        return res.status(404);
    }
    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info:"Invalid"});
        if(_uid.IP !== req.cf_ip) {
            AuthCookie.findOneAndDelete({AuthCookie: authkey}).then(b =>{});
            return res.status(201).json({Info:"Invalid"});
        }

        UserScripteDB.find({UID: _uid.UID}).then(_data =>{

            return res.status(200).json({
                Info: "Success",
                data: _data

            });
        });
    });
});

// Add a new Userscript
router.post("/add_userscript", (req, res)=> {
    const { authkey, tool, userscript, scriptName } = req.body;
    if(!authkey) {
        return res.status(404);
    }
    if(!tool) {
        return res.status(404);
    }
    if(!userscript) {
        return res.status(404);
    }
    if(tool !== process.env.CLIENTPASSWORD) {
        return res.end();
    }
    if(!tool|| !userscript|| !scriptName ) {
        return res.status(404).json({Info: "Values Missing", data: req.body});
    }
    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info:"Invalid"});
        if(_uid.IP !== req.cf_ip) {
            AuthCookie.findOneAndDelete({AuthCookie: authkey}).then(b =>{});
            return res.status(201).json({Info:"Invalid"});
        }
        const sCUID = randomstring.generate(20);
        const _NewScript = new UserScripteDB({
            name: scriptName,
            UID: _uid.UID,
            Script: userscript,
            Script_UID: sCUID
        }).save();

       return res.status(200).json({
           Info: "Success"
       });

    });
});

router.post("/delete_userscripte", (req, res) => {
    const { authkey, tool, sCUID } = req.body;
    if(!authkey) {
        return res.status(404);
    }
    if(!tool) {
        return res.status(404);
    }
    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info:"Invalid"});
        if(_uid.IP !== req.cf_ip) {
            AuthCookie.findOneAndDelete({AuthCookie: authkey}).then(b =>{});
            return res.status(201).json({Info:"Invalid"});
        }

        UserScripteDB.findOneAndDelete({Script_UID: sCUID}).then(b =>{});

        return res.status(200).json({
            Info: "Success"
        });
    });
});

//endregion

// ===================================================================================================================

                                            // Server API Stuff

// ===================================================================================================================


//region Servers

router.post("/client_new", (req, res) => {
   const { authkey, tool } = req.body;
   const {servername, port, isKEY, rsakey, ipadress, Password, ServerUsername, PassPharse, Fingerprint} = req.body;

    if(tool !== process.env.CLIENTPASSWORD) {
        return res.end();
    }
    if(!authkey) {
       return res.status(404).json({Info: "Auth Missing", data: req.body});
    }
    if(!tool) {
        return res.status(404).json({Info: "Tool Missing", data: req.body});
    }
    if(!servername || !port || !ipadress || !Password || !ServerUsername) {
        return res.status(404).json({Info: "Values Missing", data: req.body});
    }
    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info: "AuthCookie Error"});
        if(_uid.IP !== req.cf_ip) {
            AuthCookie.findOneAndDelete({AuthCookie: authkey}).then(b =>{});
            return res.status(201).json({Info:"Invalid"});
        }
        var sUID = randomstring.generate({
                length: 56,
                charset: "alphabetic"
            });
        const newServer = new sshdb({
            name: servername,
            crpyt_ServerUser: ServerUsername,
            crpyt_ip: ipadress,
            crpyt_password: Password,
            crpyt_port: port,
            crpyt_PassPharse: PassPharse,
            isKEY: isKEY,
            crypt_RSAKey: rsakey,
            server_UID: sUID,
            UID: _uid.UID,
            fingerprint: Fingerprint
        });
        newServer.save();

        return res.status(200).json({
            Info: "Success",
            ServerUID: sUID
        });
    })
});



// Delete Server

router.post("/client_delete", (req, res) => {
    const { authkey, tool } = req.body;
    const {scriptUID} = req.body;

    if(tool !== process.env.CLIENTPASSWORD) {
        return res.end();
    }
    if(!scriptUID) {
        return res.status(404).json({Info: "Script UID Missing", data: req.body, RequestIP: req.cf_ip});
    }

    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info: "AuthCookie Error"});
        if(_uid.IP !== req.cf_ip) {
            AuthCookie.findOneAndDelete({AuthCookie: authkey}).then(b =>{});
            return res.status(201).json({Info:"Invalid"});
        }
        sshdb.findOneAndDelete({server_UID: scriptUID}).then(deleter=>{});
        return res.status(200).json({
            Info: "Success"
        });
    })
});

// Add Fingerprint
router.post("/add_fingerprint", (req, res) => {
    const { authkey, tool } = req.body;
    const {serverUID, Fingerprint} = req.body;

    if(tool !== process.env.CLIENTPASSWORD) {
        return res.end();
    }
    if(!authkey) {
        return res.status(404).json({Info: "Auth Missing", RequestIP: req.cf_ip});
    }
    if(!tool) {
        return res.status(404).json({Info: "Tool Missing", RequestIP: req.cf_ip});
    }

    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info: "AuthCookie Error", RequestIP: req.cf_ip});
        if(_uid.IP !== req.cf_ip) {
            AuthCookie.findOneAndDelete({AuthCookie: authkey}).then(b =>{});
            return res.status(201).json({Info:"Invalid"});
        }
        sshdb.findOneAndUpdate({server_UID: serverUID}, {fingerprint: Fingerprint}).then(b =>{});

        return res.status(200).json({
            Info: "Success"
        });
    })


});


// Update Server

router.post("/client_update", (req, res) => {
    const { authkey, tool } = req.body;
    const {servername, port, isKEY, cryptRSAKEY, ipadress, PasswordKey, ServerUsername, PassPharse, scriptUID} = req.body;

    if(tool !== process.env.CLIENTPASSWORD) {
        return res.end();
    }
    if(!authkey) {
        return res.status(404).json({Info: "Auth Missing"});
    }
    if(!tool) {
        return res.status(404).json({Info: "Tool Missing"});
    }
    if(!servername || !port || !ipadress || !PasswordKey || !ServerUsername || !scriptUID) {
        return res.status(404).json({Info: "Values Missing", data: req.body});
    }
    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info: "AuthCookie Error"});
        if(_uid.IP !== req.cf_ip) {
            AuthCookie.findOneAndDelete({AuthCookie: authkey}).then(b =>{});
            return res.status(201).json({Info:"Invalid"});
        }
        sshdb.findByIdAndUpdate({server_UID: scriptUID}, {
            name: servername,
            crpyt_ServerUser: ServerUsername,
            crpyt_ip: ipadress,
            crpyt_password: PasswordKey,
            crpyt_port: port,
            crpyt_PassPharse: PassPharse,
            isKEY: isKEY,
            crypt_RSAKey: cryptRSAKEY

        }).then(finish => {})
        return res.status(200).json({
            Info: "Success"
        });
    })
});

//endregion

// Developer APIs.

router.get('/ip', (req, res) => {
    if(process.env.PRODUCTION) {
        res.json({
            "ClientIP Cloudflare": req.cf_ip
        })
    } else {
        return res.status(201);
    }
});


module.exports = router;