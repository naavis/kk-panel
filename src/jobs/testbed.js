var cheerio = require('cheerio');
var request = require('request');

module.exports = function(imageManager) {
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
