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
            if (err) throw err;
            if (isMatch) {
                if(user.mfa) {
                    if(!otp)
                        return res.json({Info: "2FA Error"}).status(201);
                    if(mfa(user.secret, otp)) {
                        sshdb.find({UID: user.UID}).then(sshdata => {
                            AuthCookie.findOneAndDelete({UID: user.UID}).then(()=>{});
                            const authcookie = randomstring.generate(55);
                            const new_authcookie = new AuthCookie({
                                UID: user.UID,
                                AuthCookie: authcookie,
                                IP: IPAdress
                            }).save();
                            return res.json({
                                AuthKey: authcookie,
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
                            AuthKey: authcookie,
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
    if(tool != process.env.CLIENTPASSWORD) {
        return res.end();
    }
    if(!tool|| !userscript|| !scriptName ) {
        return res.status(404).json({Info: "Values Missing", data: req.body});
    }
    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info:"Invalid"});

        const _NewScript = new UserScripteDB({
            name: scriptName,
            UID: _uid.UID,
            Script: userscript
        })

       return res.status(200).json({
           Info: "Success"
       });

    });
});

router.post("/client_new", (req, res) => {
   const { authkey, tool } = req.body;
   const {servername, port, isKEY, ipadress, PasswordKey, ServerUsername} = req.body;

    if(!authkey) {
       return res.status(404).json({Info: "Auth Missing", data: req.body});
    }
    if(!tool) {
        return res.status(404).json({Info: "Tool Missing", data: req.body});
    }
    if(tool != process.env.CLIENTPASSWORD) {
        return res.status(404).json({Info: "Clientpass", data: req.body});
    }
    if(!servername || !port || !isKEY || !ipadress || !PasswordKey || !ServerUsername) {
        return res.status(404).json({Info: "Values Missing", data: req.body});
    }
    AuthCookie.findOne({AuthCookie: authkey}).then(_uid => {
        if(!_uid)
            return res.status(201).json({Info: "AuthCookie Error"});

        const newServer = new sshdb({
            name: servername,
            crpyt_ServerUser: ServerUsername,
            crpyt_ip: ipadress,
            crpyt_password: PasswordKey,
            crpyt_port: port,
            isKEY: isKEY,
            UID: _uid.UID
        });
        newServer.save();

        return res.status(200).json({
            Info: "Success"
        });
    })
});


// Developer APIs.

router.get('/ip', (req, res) => {
    if(process.env.PRODUCTION) {
        res.json({
            "ClientIP Normal": req.ip,
            "ClientIP Cloudflare": req.cf_ip,
            ClientHeader: req.headers
        })
    } else {
        return res.status(201);
    }
});


module.exports = router;