var cheerio = require('cheerio');
var request = require('request');
var logger = require('winston');

module.exports = function(resultHandler) {
  const baseUrl = 'http://testbed.fmi.fi/?imgtype=radar&t=5&n=1';
  request(baseUrl, function(error, response, html) {
    if (error) {
      throw error;
    }
    if (response.statusCode !== 200) {
      logger.warn('Returned status code ' + response.statusCode);
      return;
    }
    let $ = cheerio.load(html);
    const imageUrl = $('img[id="anim_image_anim_anim"]').attr('src');
    resultHandler.submitImage('testbed', imageUrl);
  });
};
