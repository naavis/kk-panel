
var Scheduler = require('./scheduler.js');
var ImageManager = require('./imageManager.js');

var jobs = require('./jobs');

// Configure job schedules
var scheduler = new Scheduler();
var imageManager = new ImageManager('images/');
scheduler.add('perkkaa', '30 * * * * *', jobs.perkkaa, imageManager);
scheduler.add('metsahovi', '*/3 * * * *', jobs.metsahovi, imageManager);
scheduler.add('testbed', '*/5 * * * *', jobs.testbed, imageManager);
scheduler.add('sat24ir', '*/5 * * * *', jobs.sat24ir, imageManager);
scheduler.add('sat24vis', '*/5 * * * *', jobs.sat24vis, imageManager);
scheduler.add('kumpula', '* * * * *', jobs.kumpula, imageManager);
scheduler.startAll();
