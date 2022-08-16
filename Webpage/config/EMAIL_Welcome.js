const nodemailer = require('nodemailer');
const fs = require("fs");


module.exports = function (username, useremail) {
    const email = fs.readFile("./emails/WelcomeMail.html", (err) => {console.log(err)});
    const emailData = email.replace("{USERNAME}", username);

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
        subject : 'Welcome to BeeSSH ' + username,
        html : emailData
    };
    smtpTransport.sendMail(mailOptions, function (error, response) {
        if (error) {
            console.log(error);
        }
    });
}