var winston = require('winston');

winston.remove(winston.transports.Console);

const timestampFormat = () => { return (new Date()).toISOString(); };
winston.add(winston.transports.Console, {
  timestamp: timestampFormat,
});

module.exports = winston;
