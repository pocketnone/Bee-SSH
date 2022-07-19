const express = require('express');
const mongoose = require('mongoose');
const passport = require('passport');
const flash = require('connect-flash');
const session = require('express-session');
const helmet = require("helmet");
const favicon = require('serve-favicon');
const Static = require('serve-static');
const cloudflare = require('cloudflare-express');
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
mongoose.set('useCreateIndex', true);

// EJS
app.use(cloudflare.restore());
app.use(helmet({
    contentSecurityPolicy: false,
}));
app.set('view engine', 'ejs');
app.set('trust proxy', 2);
app.set('views', [__dirname + '/views_userpandel', __dirname + '/view_login',
    __dirname + '/view_global', __dirname + '/view_admin']);

// Express body parser
app.use(express.urlencoded({ extended: true }));

// Express session
app.use(
  session({
    secret: [process.env.SESSIONSECRET, process.env.SESSIONSECRET_1, process.env.SESSIONSECRET_2],
      name: 'beessh',
      cookie: {
        sameSite: true,
        maxAge: 86400 * 1000,
        domain: process.env.WEBPAGEDOMAIN
      },
    resave: true,
    saveUninitialized: false
  })
);

// Passport middleware
app.use(passport.initialize());
app.use(favicon(__dirname + '/ressources/img/b3.ico'));
app.use(passport.session());
app.use(express.json());
app.use((req, res, next) => {
    if (!allowedMethods.includes(req.method)) {
        return res.end(201);
    }
    next()
});

// Connect flash
app.use(flash());

// Global variables
app.use(function(req, res, next) {
  res.locals.admin_msg = req.flash('admin_msg');
  res.locals.success_msg = req.flash('success_msg');
  res.locals.error_msg = req.flash('error_msg');
  res.locals.error = req.flash('error');
  next();
});

// Routes
app.use('/', require('./routes/index.js'));
app.use('/users', require('./routes/users.js'));
app.use('/api', require('./routes/beeapi.js'));
app.use('/admin', require('./routes/admin.js'));
app.use('/assets', Static(__dirname + '/ressources'))

const PORT = 5000;

app.listen(PORT, () =>{
    console.log("Startet on Port: " + PORT);
})