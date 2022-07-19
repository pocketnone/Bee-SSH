const mongoose = require('mongoose');

const AuthCookie = new mongoose.Schema({
    UID: {
        type: String,
        required: true
    },
    IP: {
        type: String,
        required: true
    },
    AuthCookie: {
        type:String,
        required: true
    }
}, {timestamps: true});
AuthCookie.index({createdAt: 1},{expireAfterSeconds: 60 * 60 * 12}); // Delete Entry every all 12hrs
const authCookie = mongoose.model('AuthCookie', AuthCookie);

module.exports = authCookie;