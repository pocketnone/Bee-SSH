const express = require('express');
const router = express.Router();
const randomstring = require("randomstring");
const mfa = require('../config/totpAuth');

// Databases
const User = require('../models/User');
const sshdb = require('../models/SSHSessions');
const AuthCookie = require('../models/AuthCookie');

// Auth
const { ensureAuthenticated} = require('../config/auth');

// Admin GUI
router.get('/gui', ensureAuthenticated, (req, res) => {
   if(!req.user.Admin) {
        return res.redirect("/users/dashboard");
   }

    User.find().then(Users => {
        res.render('admin_gui', {
           user: req.user,
           OtherUser: Users
        });
    });
});

router.post('/delete_user', ensureAuthenticated, (req, res) => {
    if(!req.user.Admin) {
        return res.redirect("/users/dashboard");
    }

    const delete_User = req.body.userid;

    // Delete all Data from User.
    User.findOneAndDelete({UID: delete_User}).then(() =>{
        sshdb.find({UID: delete_User}).then(ToDelete => {
            ToDelete.forEach(toDelete => {
               sshdb.findOneAndDelete({UID: toDelete.UID}).then(()=>{});
            });
        });
        AuthCookie.findOneAndDelete({UID: delete_User}).then(()=>{});
    });

    req.flash(
        "admin_msg",
        "Successful Deleted user"
    );
    res.redirect('/admin/gui');
});

module.exports = router;