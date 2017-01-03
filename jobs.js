var cheerio = require('cheerio');
var request = require('request');

exports.perkkaa = function(imageManager) {
  imageManager.saveImage('perkkaa', 'https://www.puuppa.org/~pnuu/sky-cam/latest.jpg');
};

exports.metsahovi = function(imageManager) {
  imageManager.saveImage('metsahovi', 'http://data.metsahovi.fi/allsky/latest_hf.jpeg');
};

exports.testbed = function(imageManager) {
  const baseUrl = 'http://testbed.fmi.fi/';
  request(baseUrl, function(error, response, html) {
    if (error) {
      throw error;
    }
    if (response.statusCode !== 200) {
      console.log('Returned status code ' + response.statusCode);
      // TODO: Log invalid status code when logging is in place
      return;
    }
    let $ = cheerio.load(html);
    let relativeUrl = $('img[alt="Radar & temperature"]').attr('src');
    const completeUrl = baseUrl + relativeUrl;
    imageManager.saveImage('testbed', completeUrl);
  });
};

exports.sat24ir = function(imageManager) {
  saveSat24Image(imageManager, 'ir');
};

exports.sat24vis = function(imageManager) {
  saveSat24Image(imageManager, 'visual');
};

function saveSat24Image(imageManager, imageType) {
  let typeString = '';
  switch (imageType) {
    case 'ir':
      typeString = 'infraPolair';
      break;
    case 'visual':
      typeString = 'visual5';
      break;
    default:
      typeString = 'infraPolair';
      break;
  }

  const options = {
    url: 'http://en.sat24.com/en/scan/' + typeString,
    headers: {
      'User-Agent': 'request'
    }
  };
  request(options, function(error, response, html) {
    if (error) {
      throw error;
    }
    if (response.statusCode !== 200) {
      console.log('Returned status code ' + response.statusCode);
      // TODO: Log invalid status code when logging is in place
      return;
    }
    let $ = cheerio.load(html);
    const embeddedScript = $('script[type="text/javascript"]', '#content').first().text();
    const splitScript = embeddedScript.split(';');
    const latestTimestamp = splitScript[splitScript.length - 2].split('\'')[1];
    const imageUrl = 'http://en.sat24.com/image?type=' + typeString + '&region=scan&timestamp=' + latestTimestamp;
    imageManager.saveImage('sat24' + imageType, imageUrl);
  });
}
