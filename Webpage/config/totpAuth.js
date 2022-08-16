const Speakeasy = require('speakeasy');


module.exports = function(secret, token) {
    const isValis = Speakeasy.totp.verify({
        secret: secret,
        encoding: "base32",
        token: token,
        window: 0
    });
    return isValis; // return true or false
}