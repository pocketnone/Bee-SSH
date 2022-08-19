const express = require('express');
const Speakeasy = require('speakeasy');
const mfa = require('../config/totpAuth');
const QRCode = require('qrcode');
const { passwordStrength } = require('check-password-strength')
const pwStrength = passwordStrength;
const router = express.Router();
const { ensureAuthenticated, forwardAuthenticated } = require('../config/auth');


// Models
const User = require('../models/User');
const sshdb = require('../models/SSHSessions');
const AuthCookie = require('../models/AuthCookie');
const UserScripteDB = require('../models/CustomUserScripts');
const bcrypt = require("bcryptjs");

// Welcome Page
router.get('/', forwardAuthenticated, (req, res) => res.render('welcome'));

// Dashboard
router.get('/dashboard', ensureAuthenticated, (req, res) =>
    UserScripteDB.find({UID: req.user.UID}).then(scripts => {
        sshdb.find({UID: req.user.UID}).then(servers => {
            User.find({}).then(data => {
                res.render('dashboard', {
                    user: req.user,
                    server: servers.length,
                    script: scripts.length,
                    current_user: data.length
                })
            })
        })
    })
);


router.get('/setup-2fa', ensureAuthenticated, (req,res) => {
    if(req.user.mfa) {
        return res.render('edit_profile', {
            user: req.user
        });
    }

    const secret = Speakeasy.generateSecret({length: 30});

    User.findOneAndUpdate({UID: req.user.UID}, {secret: secret.base32}).then(b => {});
        QRCode.toDataURL(secret.otpauth_url, function(err, data_url) {
            return res.render('2FA-Setup', {
                user: req.user,
                otp_secret: secret.base32,
                qrcode: data_url
            });
    });
});

router.post("/setup-2fa-validate", ensureAuthenticated, (req, res) =>{
    const token = req.body.token;
    if(!token)
        return res.redirect("/dashboard");
    if(req.user.mfa)
        return res.redirect("/setup-2fa");

    if(mfa(req.user.secret, token)) {
        User.findByIdAndUpdate(req.user._id, {mfa: true}).then(b =>{});
        req.logout(function(err) {
            if (err) { return next(err); }
            req.flash('success_msg', '2FA Aktivated and Regenerated AuthCookie. Please login back.');
            return res.redirect('/users/login');
        });
    } else {
        return res.redirect("/setup-2fa");
    }
});


router.post("/change_password", ensureAuthenticated, (req, res) =>{
    const {cirrent_password, password, password_2, token} = req.body;

    let errors = [];

    if(!cirrent_password, !password, !password_2) {
        errors.push({ msg: 'Please fill out every Password Box' });
    }
    if(password != password_2) {
        errors.push({ msg: 'Password do not match' });
    }

    if(pwStrength(password).value !== 'Strong'){
        errors.push({ msg: 'Your password is too weak. A calculator could bruteforce it' });
    }

    // User have 2FA
    if(req.user.mfa){
        if(mfa(req.user.secret, token)) {
            bcrypt.compare(cirrent_password + process.env.PASSPEPPER, req.user.password, (err, isMatch) =>{
                if(err) console.log(err);

                if(isMatch) {
                    bcrypt.genSalt(16, (err, salt) => {
                        bcrypt.hash(password + process.env.PASSPEPPER, salt, (err, hash) => {
                            if(err) throw err;
                            User.findByIdAndUpdate(req.user._id, {password: hash}).then(b =>{});
                            req.flash('success_msg', 'Successful changed Password');
                            return res.redirect('/edit_profile');
                        });
                    });

                } else {
                    errors.push({ msg: 'Password False' });
                    res.render('edit_profile', {
                        errors,
                        user: req.user
                    });
                }
            });
        } else {
            errors.push({ msg: '2FA Code Invalid' });
            res.render('edit_profile', {
                errors,
                user: req.user
            });
        }
    }


    if(!req.user.mfa)
    {
        bcrypt.compare(cirrent_password + process.env.PASSPEPPER, req.user.password, (err, isMatch) =>{
            if(err) console.log(err);

            if(isMatch) {
                bcrypt.genSalt(16, (err, salt) => {
                    bcrypt.hash(password + process.env.PASSPEPPER, salt, (err, hash) => {
                        if(err) throw err;
                        User.findByIdAndUpdate(req.user._id, {password: hash}).then(b =>{});
                        req.flash('success_msg', 'Successful changed Password');
                        return res.redirect('/edit_profile');
                    });
                });

            } else {
                errors.push({ msg: 'Password False' });
                res.render('edit_profile', {
                    errors,
                    user: req.user
                });
            }
        });
    }
});



module.exports = router;
