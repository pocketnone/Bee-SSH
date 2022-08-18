const nodemailer = require('nodemailer');
const fs = require('fs');

module.exports = function (username, useremail, resettoken, ip_adress) {
    fs.readFile(__dirname + "/emails/PasswortReset.html", 'utf8', (err, data) => {
        if (err) {
            return console.log(err);
        }
        const emailData = data.replace("{USERNAME}", username).replace("{ipadressbox}", ip_adress).replace("\"[RESETTOKKEN]",  resettoken + "\"").replace("\"[RESETTOKKEN]",  resettoken + "\"");



        const smtpTransport = nodemailer.createTransport({
            host: process.env.EMAILHOST,
            secureConnection: false,
            port: process.env.MAILPORT,
            auth: {
                user: process.env.MAILUSERNAME,
                pass: process.env.MAILUSERPASS
            },
            tls: {
                ciphers:'SSLv3',
                rejectUnauthorized: false
            }
        });
        const mailOptions = {
            from: process.env.MAILUSERNAME,
            to : useremail,
            subject : 'Reset Password',
            html : emailData
        };
        smtpTransport.sendMail(mailOptions, function (error, response) {
            if (error) {
                console.log(error);
            }
        });
    });
}