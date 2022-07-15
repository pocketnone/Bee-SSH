const express = require('express');
const Speakeasy = require('speakeasy');
const mfa = require('../config/totpAuth');
const router = express.Router();
const { ensureAuthenticated, forwardAuthenticated } = require('../config/auth');


// Models
const User = require('../models/User');

// Welcome Page
router.get('/', forwardAuthenticated, (req, res) => res.render('welcome'));
router.get('/download', forwardAuthenticated, (req, res) => res.render('download'));
router.get('/devs', forwardAuthenticated, (req, res) => res.render('devs'));
router.get('/features', forwardAuthenticated, (req, res) => res.render('features'));
router.get('/faq', forwardAuthenticated, (req, res) => res.render('faq'));

// Dashboard
router.get('/dashboard', ensureAuthenticated, (req, res) =>
  res.render('dashboard', {
    user: req.user
  })
);

router.get('/edit_profile', ensureAuthenticated, (req, res) =>
    res.render('edit_profile', {
        user: req.user
    })
);

router.get('/setup-2fa', ensureAuthenticated, (req,res) => {
    if(req.user.mfa) {
        return res.render('edit_profile', {
            user: req.user
        });
    }

    const secret = Speakeasy.generateSecret({length: 30});

    User.findOneAndUpdate({UID: req.user.UID}, {secret: secret.base32});

    return res.render('2FA-Setup', {
        user: req.user,
        otp_secret: secret.base32,
        qrcode: secret.google_auth_qr
    });
});

router.post("/setup-2fa-validate", ensureAuthenticated, (req, res) =>{
    const token = req.body.token;
    if(!token)
        return res.redirect("/users/dashboard");
    if(req.user.mfa)
        return res.redirect("/users/setup-2fa");

    if(mfa(req.user.secret, token)) {
        User.findByIdAndUpdate(req.user._id, {mfa: true});
        req.logout();
        req.flash('success_msg', '2FA Aktivated and Regenerated AuthCookie. Please login back.');
        return res.redirect('/users/login');
    } else {
        return res.redirect("/users/setup-2fa");
    }
});

module.exports = router;
