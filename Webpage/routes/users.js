const express = require('express');
const router = express.Router();
const bcrypt = require('bcryptjs');
const passport = require('passport');
const {verify} = require('hcaptcha');
// Load User model
const User = require('../models/User');
const mfa = require('../config/totpAuth');
const { forwardAuthenticated } = require('../config/auth');

// Login Page
router.get('/login', forwardAuthenticated, (req, res) => res.render('login'));

// Register Page
router.get('/register', forwardAuthenticated, (req, res) => res.render('register'));

// Register
router.post('/register', (req, res) => {
  const htoken = req.POST_DATA['h-captcha-response'];
  const hsecret = process.env.HCAPTCHATOKEN;
  const { name, email, password, password2 } = req.body;
  let errors = [];

  if (!name || !email || !password || !password2) {
    errors.push({ msg: 'Please enter all fields' });
  }

  if (password != password2) {
    errors.push({ msg: 'Passwords do not match' });
  }

  if (password.length < 10) {
    errors.push({ msg: 'Password must be at least 6 characters' });
  }

  if (errors.length > 0) {
    res.render('register', {
      errors,
      name,
      email,
      password,
      password2
    });
  } else {
    User.findOne({ email: email }).then(user => {
      if (user) {
        errors.push({ msg: 'Email already exists' });
        res.render('register', {
          errors,
          name,
          email,
          password,
          password2
        });
      } else {
        const newUser = new User({
          name,
          email,
          password
        });

        bcrypt.genSalt(16, (err, salt) => {
          bcrypt.hash(newUser.password + process.env.PASSPEPPER, salt, (err, hash) => {
            if (err) throw err;
            newUser.password = hash;
            newUser
              .save()
              .then(user => {
                req.flash(
                  'success_msg',
                  'You are now registered and can log in'
                );
                res.redirect('/users/login');
              })
              .catch(err => console.log(err));
          });
        });
      }
    });
  }
});

// Login
router.post('/login', (req, res, next) => {
  const secret = req.body.mfakey;
  const mail = req.body.email;
  const htoken = req.POST_DATA['h-captcha-response'];
  const hsecret = process.env.HCAPTCHATOKEN;
  verify(hsecret, htoken)
      .then((data) => {
        if (data.success === true) {
          User.findOne({email: mail}).then(user=>{
            if(user){
              if(user.mfa == true) {                        // User use 2FA
                if(mfa(user.secret, secret)) {
                  passport.authenticate('local', {
                    successRedirect: '/dashboard',
                    failureRedirect: '/users/login',
                    failureFlash: true
                  })(req, res, next);
                } else {
                  req.flash(
                      'error_msg',
                      'Something went wrong'
                  );
                  res.redirect('/users/login');
                }
              } else {
                passport.authenticate('local', {
                  successRedirect: '/dashboard',
                  failureRedirect: '/users/login',
                  failureFlash: true
                })(req, res, next);
              }
              req.flash(
                  'error_msg',
                  'Something went wrong'
              );
              res.redirect('/users/login');
            } else {
              req.flash(
                  'error_msg',
                  'User didnt Exist. Please create an Account.'
              );
              res.redirect('/users/register');
            }
          })
        } else {
          req.flash(
              'error_msg',
              'hCaptcha Failed'
          );
          res.redirect('/users/login');
        }
      })
      .catch(console.error);

});

// Logout
router.get('/logout', (req, res) => {
  req.logout();
  req.flash('success_msg', 'You are logged out');
  res.redirect('/users/login');
});

module.exports = router;
