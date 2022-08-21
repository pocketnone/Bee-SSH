const mongoose = require('mongoose');

const SSH = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    crpyt_ServerUser: {
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
    crpyt_PassPharse: {
        type: String,
        required: true,
        default: "NULL"
    },
    isKey: {
        type: Boolean,
        default: false,
        required: true
    },
    UID: {
        type: String,
        required: true
    },
    script_UID: {
        type: String,
        required: true
    }
});

const UserSSH = mongoose.model('UserSSH', SSH);

module.exports = UserSSH;
