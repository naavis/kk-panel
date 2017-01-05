var path = require('path');
var express = require('express');
var app = express();
var server = require('http').Server(app);
var io = require('socket.io')(server);

var Scheduler = require('./scheduler.js');
var ImageManager = require('./imageManager.js');

var jobs = require('./jobs');

const PORT = 3000;

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

// Configure Express HTTP server
app.set('view engine', 'pug');
app.use('/images', express.static(path.join(__dirname, 'images')));
app.use(express.static(path.join(__dirname, 'style')));

// Render main page
app.get('/', function(req, res) {
  res.render('index');
});

// Initialize server
server.listen(PORT, function() {
  console.log(`Listening on port ${PORT}!`);
});
