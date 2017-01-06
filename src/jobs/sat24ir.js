var common = require('./sat24common.js');

module.exports = function(imageManager) {
  common.submitSat24Image(imageManager, 'ir');
};
