var cheerio = require('cheerio');
var request = require('request');
var logger = require('winston');

exports.submitSat24Image = function (resultHandler, imageType) {
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
      logger.warn('Returned status code ' + response.statusCode);
      return;
    }
    let $ = cheerio.load(html);
    const embeddedScript = $('script[type="text/javascript"]', '#content').first().text();
    const latestTimestamp = parseEmbeddedSat24Script(embeddedScript);
    const imageUrl = 'http://en.sat24.com/image?type=' + typeString + '&region=scan&timestamp=' + latestTimestamp;
    resultHandler.submitImage('sat24' + imageType, imageUrl);
  });
};

getSat24TypeString = function (imageType) {
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
};

parseEmbeddedSat24Script = function (scriptContents) {
  const splitScript = scriptContents.split(';');
  const latestTimestamp = splitScript[splitScript.length - 2].split('\'')[1];
  return latestTimestamp;
};
