const mongoose = require('mongoose');

const ResetTokens = new mongoose.Schema({
    UID: {
        type: String,
        required: true
    },
    ResetToken: {
        type:String,
        required: true
    }
}, {timestamps: true});
ResetTokens.index({createdAt: 1},{expireAfterSeconds: 60 * 5}); // Delete Token after 5min.
const ResetToken = mongoose.model('ResetToken', ResetTokens);

module.exports = ResetToken;