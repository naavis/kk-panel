var cron = require('node-cron');

var ImageManager = require('./imageManager.js');
var imageManager = new ImageManager('images/');

var jobs = require('./jobs.js');

// Configure job schedules
cron.schedule('30 * * * * *', function perkkaaJob() {
  console.log('Starting Perkkaa job!');
  jobs.perkkaa(imageManager);
});
cron.schedule('*/3 * * * *', function metsahoviJob() {
  console.log('Starting Metsähovi job!');
  jobs.metsahovi(imageManager);
});
cron.schedule('*/5 * * * *', function metsahoviJob() {
  console.log('Starting Testbed job!');
  jobs.testbed(imageManager);
});
cron.schedule('*/5 * * * *', function sat24irJob() {
  console.log('Starting Sat24 IR job!');
  jobs.sat24ir(imageManager);
});
cron.schedule('*/5 * * * *', function sat24irJob() {
  console.log('Starting Sat24 Visual job!');
  jobs.sat24vis(imageManager);
});
