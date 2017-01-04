var assert = require('assert');
var rewire = require('rewire');
var jobs = rewire('../jobs.js');

describe('jobs', () => {
  describe('parseEmbeddedSat24Script', () => {
    it('parses correct timestamp from script', () => {
      let parseEmbeddedSat24Script = jobs.__get__('parseEmbeddedSat24Script');
      let timestamp = parseEmbeddedSat24Script(embeddedScript);

      assert(timestamp === '201701041205');
    });

    // Script taken from Sat24 website
    const embeddedScript =
        `var region = "" + 'scan' + "";
        var imageType = "" + 'visual' + "";
        var imageCount = 0 + 10 +0;
        var allowZoom = 'True' == 'True';

        var AmazonUrl = "" + '' + "";
        var satbeelden = 0 + 10 +0;
        var zoomX = 0;
        var zoomY = 0;
        var iszoom = false;

        var arrayImageTimes = [];
        arrayImageTimes.push('201701040950');arrayImageTimes.push('201701041005');arrayImageTimes.push('201701041020');arrayImageTimes.push('201701041035');arrayImageTimes.push('201701041050');arrayImageTimes.push('201701041105');arrayImageTimes.push('201701041120');arrayImageTimes.push('201701041135');arrayImageTimes.push('201701041150');arrayImageTimes.push('201701041205');`
  });
});
