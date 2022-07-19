const nodemailer = require('nodemailer');
const fs = require('fs');

module.exports = function (username, useremail, resettoken, ip_adress) {
    const email = fs.readFile("./emails/PasswortReset.html", (err) => {console.log(err)}).toString();
    const emailData = email.replace("{USERNAME}", username).replace("{resettoken}", resettoken).replace("{ipadressbox}", ip_adress);


    const smtpTransport = nodemailer.createTransport(smtpTransport({
        host: process.env.EMAILHOST,
        secure: true,
        port: process.env.MAILPORT,
        auth: {
            user: process.env.MAILUSERNAME,
            pass: process.env.MAILUSERPASS
        }
    }));
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
}