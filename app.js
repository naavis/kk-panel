var cron = require('node-cron');

var ImageSaver = require('./image-saver.js');
var imageSaver = new ImageSaver('images/');

var jobs = require('./jobs.js');

// Configure job schedules
cron.schedule('30 * * * * *', function perkkaaJob() {
  console.log('Starting Perkkaa job!');
  jobs.perkkaa(imageSaver);
});
cron.schedule('*/3 * * * *', function metsahoviJob() {
  console.log('Starting Metsähovi job!');
  jobs.metsahovi(imageSaver);
});
cron.schedule('*/5 * * * *', function metsahoviJob() {
  console.log('Starting Testbed job!');
  jobs.testbed(imageSaver);
});
cron.schedule('*/5 * * * *', function sat24irJob() {
  console.log('Starting Sat24 IR job!');
  jobs.sat24ir(imageSaver);
});
cron.schedule('*/5 * * * *', function sat24irJob() {
  console.log('Starting Sat24 Visual job!');
  jobs.sat24vis(imageSaver);
});
