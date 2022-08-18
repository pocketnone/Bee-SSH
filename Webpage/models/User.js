const mongoose = require('mongoose');

const UserSchema = new mongoose.Schema({
  name: {
    type: String,
    required: true
  },
  secret: {
    type: String,
    default: "Data",
    required: true
  },
  mfa: {
    type: Boolean,
    required: true,
    default: false
  },
  Admin: {
    type: Boolean,
    required: true,
    default: false
  },
  UID: {
    type: String,
    required: true
  },
  email: {
    type: String,
    required: true,
    unique: true
  },
  password: {
    type: String,
    required: true
  },
  CreationDate: {
    type: Date,
    default: Date.now(),
    required: true
  }
});

const User = mongoose.model('User', UserSchema);

module.exports = User;
