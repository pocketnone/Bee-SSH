const mongoose = require('mongoose');

const UserScripte = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    UID: {
        type: String,
        required: true
    },
    Script: {
        type: String,
        required: true
    },
    Script_UID: {
        type: String,
        required: true
    }
});

const UserSSHScript = mongoose.model('UserSSHScripte', UserScripte);

module.exports = UserSSHScript;
