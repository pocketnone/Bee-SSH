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
    isRSA: {
        type: Boolean,
        default: false,
        required: true
    },
    crypt_RSAKey: {
        type: String,
        default: "nodata",
        required: true
    },
    UID: {
        type: String,
        required: true
    },
    server_UID: {
        type: String,
        required: true
    },
    fingerprint: {
        type: String,
        required: true,
        default: 'null'
    }
});

const UserSSH = mongoose.model('UserSSH', SSH);

module.exports = UserSSH;
