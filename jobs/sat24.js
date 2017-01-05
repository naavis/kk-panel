var cheerio = require('cheerio');
var request = require('request');

exports.sat24ir = function(imageManager) {
  saveSat24Image(imageManager, 'ir');
};

exports.sat24visual = function(imageManager) {
  saveSat24Image(imageManager, 'visual');
};

function saveSat24Image(imageManager, imageType) {
  // typeString is used in URLs to differentiate between IR images and Visual images
  let typeString = getSat24TypeString(imageType);

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
    const latestTimestamp = parseEmbeddedSat24Script(embeddedScript);
    const imageUrl = 'http://en.sat24.com/image?type=' + typeString + '&region=scan&timestamp=' + latestTimestamp;
    imageManager.saveImage('sat24' + imageType, imageUrl);
  });
}

function getSat24TypeString(imageType) {
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
  return typeString;
}

function parseEmbeddedSat24Script(scriptContents) {
  const splitScript = scriptContents.split(';');
  const latestTimestamp = splitScript[splitScript.length - 2].split('\'')[1];
  return latestTimestamp;
}
