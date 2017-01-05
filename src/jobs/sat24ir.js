var common = require('./sat24common.js');

module.exports = function(imageManager) {
  common.saveSat24Image(imageManager, 'ir');
};
