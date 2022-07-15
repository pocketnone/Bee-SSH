const mongoose = require('mongoose');

const AuthCookie = new mongoose.Schema({
    UID: {
        type: String,
        required: true
    },
    AuthCookie: {
        type:String,
        required: true
    }
});

const authCookie = mongoose.model('AuthCookie', AuthCookie);

module.exports = authCookie;
