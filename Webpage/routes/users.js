const express = require('express');
const router = express.Router();
const bcrypt = require('bcryptjs');
const passport = require('passport');

// Webpage Security
const {verify} = require('hcaptcha');
const xssFilters = require('sanitize-html');
const { passwordStrength } = require('check-password-strength')
const pwStrength = passwordStrength;
const randomstring = require("randomstring");

// E-Mail Functions
const WelcomeMail = require('../config/EMAIL_Welcome');
const ResetEmail = require('../config/EMAIL_ResetPasswort');

// Load User model
const User = require('../models/User');
const mfa = require('../config/totpAuth');
const resetDB = require('../models/ResetPassworkTokenDB');
const { forwardAuthenticated } = require('../config/auth');

// Login Page
router.get('/login', forwardAuthenticated, (req, res) => res.render('login'));

// Register Page
router.get('/register', forwardAuthenticated, (req, res) => res.render('register'));

// Register
router.post('/register', (req, res) => {
  const htoken = req.body['h-captcha-response'];
  const hsecret = process.env.HCAPTCHASECRET;
  const UID = randomstring.generate(20);
  const { name, email, password, password2 } = req.body;
  let errors = [];

  // All field are filled?
  if (!xssFilters(name) || !email || !password || !password2) {
    errors.push({ msg: 'Please enter all fields' });
  }
  // Regex Name
  if (/[^a-zA-Z]/.test(name)){
    errors.push({ msg: 'Only alphabetical symbols are allowed' });
  }
  // Lenght Check
  if(name.length >= 16){
    errors.push({ msg: 'Name is too long' });
  }

  if (password != password2) {
    errors.push({ msg: 'Passwords do not match' });
  }

  if (password.length < 10) {
    errors.push({ msg: 'Password must be at least 10 characters' });
  }
  // Check PW strength
  if(pwStrength(password).value !== 'Strong'){
    errors.push({ msg: 'Your password is too weak. A calculator could bruteforce it' });
  }
  verify(hsecret, htoken).then((data) => {
    if (data.success === false) {
      errors.push({ msg: 'hCaptcha Error' });
    }}).catch(console.error);


  if (errors.length > 0) {
    return res.render('register', {
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
          return res.render('register', {
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
          UID,
          password
        });

        bcrypt.genSalt(16, (err, salt) => {
          bcrypt.hash(newUser.password + process.env.PASSPEPPER, salt, (err, hash) => {
            if (err) throw err;
            newUser.password = hash;
            newUser
              .save()
              .then(user => {
                WelcomeMail(name, email);
                req.flash(
                  'success_msg',
                  'You are now registered and can log in'
                );
                return res.redirect('/users/login');
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
  const htoken = req.body['h-captcha-response'];
  const hsecret = process.env.HCAPTCHASECRET;

  verify(hsecret, htoken).then((data) => {
    if (data.success === false) {
      req.flash(
          'error_msg',
          'hCaptcha Error'
      );
      return res.redirect('/users/login');
    } else {
        User.findOne({email: mail}).then(user=>{
            if(user) {
                if (user.mfa == true) {  // User use 2FA
                    if (mfa(user.secret , secret)) {
                        passport.authenticate('local', {
                            successRedirect: '/dashboard',
                            failureRedirect: '/users/login',
                            failureFlash: true
                        })(req, res, next);
                    } else {
                        req.flash(
                            'error_msg',
                            'User, Password ore 2FA Code are Incorrect'
                        );
                        return res.redirect('/users/login');
                    }
                } else {
                    passport.authenticate('local', {
                        successRedirect: '/dashboard',
                        failureRedirect: '/users/login',
                        failureFlash: true
                    })(req, res, next);
                }
            } else {
                req.flash(
                    'error_msg',
                    'User or Password are Incorrect'
                );
                return res.redirect('/users/login');
            }
        });
    }

  }).catch(console.error);

});

// Logout
router.get('/logout', (req, res) => {
  req.logout(function(err) {
      if (err) { return next(err); }
      req.flash('success_msg', 'You are logged out');
      res.redirect('/users/login');
  });
});


// Reset Passwords

// Request Password Page
router.get('/resetpassword', forwardAuthenticated, (req, res) => {
     res.render('PasswordResetRequest');
});

router.post('/requestreset', forwardAuthenticated, (req, res) =>{
      const email = req.body.email;
      const htoken = req.body['h-captcha-response'];
      const IPAdresse = req.cf_ip;
      const hsecret = process.env.HCAPTCHASECRET;

      let errors = [];

      if(!email) {
        errors.push({ msg: 'Please enter all fields' });
      }

      verify(hsecret, htoken).then((data) => {
        if (data.success === false) {
          errors.push({ msg: 'hCaptcha Error' });
        }}).catch(console.error);

      if(errors > 0) {
        return res.render('PasswordResetRequest', { errors });
      }

      User.findOne({email: email}).then(user => {
          if(!user) {
            errors.push({ msg: 'E-mail dont exist' });
            return res.render('PasswordResetRequest', { errors });
          }
          const ResetTokenString = randomstring.generate(25);
          // Create new Reset Token
          const _ResetToken = new resetDB({
            UID: user.UID,
            ResetToken: ResetTokenString
          });
          // Reset Email
          ResetEmail(user.name, user.email, ResetTokenString, IPAdresse);

          //Save new DB Entry
          _ResetToken.save();

        req.flash(
            'success_msg',
            'Please check you E-Mail to reset your Password'
        );
        res.redirect('/users/resetpassword');
      });
});



// Change Password
router.get('/complete/:ResetToken', forwardAuthenticated, (req, res) =>{
      const resetToken = xssFilters(req.params.ResetToken);
      let errors = [];

      if(!resetToken) {
        errors.push({ msg: 'Please request a Password-Reset' });
      }

      if(resetToken.length !== 25) {
        errors.push({ msg: 'Please request a Password-Reset' });
      }

      if(errors > 0) {
        return res.render('PasswordResetRequest', { errors });
      }

      resetDB.findOne({ResetToken: resetToken}).then(_uidUser => {
          if(!_uidUser) {
              errors.push({ msg: 'Please request a Password-Reset' });
              console.log(errors);
              return res.render('PasswordResetRequest', { errors });
          }

          User.findOne({UID: _uidUser.UID}).then(_user => {

              res.render('PasswordResetComplete', {
                  user: _user,
                  ResetToken: resetToken
              })
          })
      })

});

router.post('/changePassword', forwardAuthenticated, (req, res) => {
    let { password, password_1, resettoken, mfakey} = req.body;
    let errors = [];

    if(!resetToken) {
        errors.push({ msg: 'Please request a Password-Reset' });
    }

    if(resetToken.length !== 25) {
        errors.push({ msg: 'Please request a Password-Reset' });
    }

    if (password != password_1) {
        errors.push({ msg: 'Passwords do not match' });
    }
    // PW Length
    if (password.length < 10) {
        errors.push({ msg: 'Password must be at least 10 characters' });
    }
    // Check PW strength
    if(pwStrength(password).value !== 'Strong'){
        errors.push({ msg: 'Your password is too weak. A calculator could bruteforce it' });
    }

    if(errors > 0) {
        req.flash(
            errors
        );
        return res.redirect('/users/complete/' + resettoken);
    }

    resetDB.findOne({ResetToken: resettoken}).then(_uidUser => {
        if(!_uidUser) {
            errors.push({ msg: 'Please request a Password-Reset' });
            req.flash(
                errors
            );
            return res.redirect('/users/complete/' + resettoken);
        }

        User.findOne({UID: _uidUser.UID}).then(_user => {
           if(_user.mfa) {
               if(!mfakey)
                   mfakey = "00000"; //Ugly Cheat ;D
               if(!mfa(_user.secret, mfakey)) {
                   errors.push({ msg: '2FA Code Failed' });
                   req.flash(
                       errors
                   );
                   return res.redirect('/users/complete/' + resettoken);
               }
           }
            bcrypt.genSalt(16, (err, salt) => {
                bcrypt.hash(password + process.env.PASSPEPPER, salt, (err, hash) => {
                    if (err) throw err;
                    User.findOneAndUpdate({UID: _user.UID}, {password: hash}).then(()=>{});
                    req.flash(
                        'success_msg',
                        'Successfully Changed Password'
                    );
                    res.redirect('/users/login');
                });
            });

        })
    })
})


module.exports = router;
