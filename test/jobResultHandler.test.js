var assert = require('assert');
var sinon = require('sinon');
var JobResultHandler = require('../src/jobResultHandler.js');

describe('JobResultHandler', () => {
  var imageManagerSpy = {};
  var ioSpy = {};
  var jobResultHandler = {};
  beforeEach(() => {
    imageManagerSpy = {
      saveImage: sinon.spy()
    };
    ioSpy = {
      emit: sinon.spy()
    };
    jobResultHandler = new JobResultHandler(ioSpy, imageManagerSpy);
  });

  describe('#submitImage', () => {
    it('calls ImageManager to save image', () => {
      jobResultHandler.submitImage('dummyId', 'dummyUrl');

      assert(imageManagerSpy.saveImage.calledOnce);
      let [id, url] = imageManagerSpy.saveImage.args[0];
      assert.equal(id, 'dummyId');
      assert.equal(url, 'dummyUrl');
    });
  });
});
