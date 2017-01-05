var fs = require('fs');
var path = require('path');
var logger = require('./logger.js');

var express = require('express');
var app = express();
var server = require('http').Server(app);
var io = require('socket.io')(server);

var Scheduler = require('./scheduler.js');
var ImageManager = require('./imageManager.js');

var jobs = require('./jobs');

const PORT = 3000;

let panelConfig = {};
try {
  panelConfig = JSON.parse(fs.readFileSync('panelConfig.json', 'utf8'));
} catch (e) {
  logger.info('Config file is not valid JSON: ' + e);
  process.exit(-1);
}

// Configure job schedules
var imageManager = new ImageManager('images/');
var scheduler = new Scheduler(panelConfig.panels, imageManager);
scheduler.startAll();

// Configure Express HTTP server
app.set('view engine', 'pug');
app.use('/images', express.static(path.join(__dirname, '../images')));
app.use(express.static(path.join(__dirname, '../static')));

// Render main page
app.get('/', function(req, res) {
  res.render('index', panelConfig);
});

// Initialize server
server.listen(PORT, function() {
  logger.info(`Listening on port ${PORT}!`);
});
