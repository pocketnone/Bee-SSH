const mongoose = require('mongoose');

const SSH = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    crpyt_ip: {
        type: String,
        required: true
    },
    crpyt_password: {
        type: String,
        required: true
    },
    crpyt_port: {
        type: String,
        required: true
    },
    isKey: {
        type: Boolean,
        default: false,
        required: true
    },
    UID: {
        type: String,
        required: true
    }
});

const UserSSH = mongoose.model('UserSSH', SSH);

module.exports = UserSSH;
