var cheerio = require('cheerio');
var request = require('request');
var logger = require('winston');

module.exports = function(imageManager) {
  const baseUrl = 'http://testbed.fmi.fi/';
  request(baseUrl, function(error, response, html) {
    if (error) {
      throw error;
    }
    if (response.statusCode !== 200) {
      logger.warn('Returned status code ' + response.statusCode);
      return;
    }
    let $ = cheerio.load(html);
    let relativeUrl = $('img[alt="Radar & temperature"]').attr('src');
    const completeUrl = baseUrl + relativeUrl;
    imageManager.saveImage('testbed', completeUrl);
  });
};
