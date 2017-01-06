var winston = require('winston');
winston.remove(winston.transports.Console);

let timestampFormat = () => ((new Date()).toISOString());
let options = {
  timestamp: timestampFormat,
}

if (process.env.NODE_ENV === 'production') {
  options.filename = 'app.log';
  winston.add(winston.transports.File, options);
} else {
  winston.add(winston.transports.Console, options);
}

module.exports = winston;
