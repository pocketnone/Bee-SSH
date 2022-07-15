const express = require('express');
const mongoose = require('mongoose');
const passport = require('passport');
const flash = require('connect-flash');
const session = require('express-session');
const helmet = require("helmet");
require('dotenv').config();
const app = express();

const allowedMethods = ['GET', 'POST'];
// Passport Config
require('./config/passport')(passport);

// DB Config
const db = process.env.MONGODBURL;

// Connect to MongoDB
mongoose
  .connect(db, { useNewUrlParser: true, useUnifiedTopology: true })
  .then(() => console.log('MongoDB Connected'))
  .catch(err => console.log(err));

// EJS
app.use(helmet({
    contentSecurityPolicy: false,
}));
app.set('view engine', 'ejs');
app.set('views', [__dirname + '\\views_userpandel', __dirname + '\\view_login',
    __dirname + '\\view_global', __dirname + '\\view_admin']);

// Express body parser
app.use(express.urlencoded({ extended: true }));

// Express session
app.use(
  session({
    secret: [process.env.SESSIONSECRET, process.env.SESSIONSECRET_1, process.env.SESSIONSECRET_2],
      name: 'beessh',
      cookie: {
        sameSite: true, maxAge: 86400 * 1000
      },
    resave: true
  })
);

// Passport middleware
app.use(passport.initialize());
app.use(passport.session());
app.use(express.json());
app.use((req, res, next) => {
    if (!allowedMethods.includes(req.method)) {
        return res.end(401);
    }
    next()
});

// Connect flash
app.use(flash());

// Global variables
app.use(function(req, res, next) {
  res.locals.success_msg = req.flash('success_msg');
  res.locals.error_msg = req.flash('error_msg');
  res.locals.error = req.flash('error');
  next();
});

// Routes
app.use('/', require('./routes/index.js'));
app.use('/users', require('./routes/users.js'));
app.use('/api', require('./routes/beeapi.js'));
app.use(express.static('./ressources'));

const PORT = 5000;

app.listen(PORT, ()=> {
    console.log(`Listening as : ${PORT}`);
});
