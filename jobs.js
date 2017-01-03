var cheerio = require('cheerio');
var request = require('request');

exports.perkkaa = function(imageSaver) {
  imageSaver.saveImage('perkkaa', 'https://www.puuppa.org/~pnuu/sky-cam/latest.jpg');
};

exports.metsahovi = function(imageSaver) {
  imageSaver.saveImage('metsahovi', 'http://data.metsahovi.fi/allsky/latest_hf.jpeg');
};

exports.testbed = function(imageSaver) {
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
    imageSaver.saveImage('testbed', completeUrl);
  });
};

exports.sat24ir = function(imageSaver) {
  saveSat24Image(imageSaver, 'ir');
};

exports.sat24vis = function(imageSaver) {
  saveSat24Image(imageSaver, 'visual');
};

function saveSat24Image(imageSaver, imageType) {
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
    imageSaver.saveImage('sat24' + imageType, imageUrl);
  });
}
