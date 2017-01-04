var sinon = require('sinon');
var path = require('path');
var mock = require('mock-require');
var assert = require('assert');
var ImageManager = require('../imageManager.js');

const DUMMY_NAME = 'dummyname';
const DUMMY_PATH = 'dummypath';
const DUMMY_DEST = path.join('dummypath', 'dummyname-latest.jpg');
const DUMMY_URL = 'http://foo.bar/baz.jpg';
const DUMMY_URL_NO_EXT = 'http://foo.bar/baz';

describe('imageManager', () => {
  var manager = {};
  var downloaderSpy = {};
  var fsSpy = {};

  beforeEach(() => {
    fsSpy = { statSync: sinon.spy() };
    mock('fs', fsSpy);

    downloaderSpy = sinon.spy();
    mock('image-downloader', downloaderSpy);
    ImageManager = mock.reRequire('../imageManager.js');

    manager = new ImageManager(DUMMY_PATH);
  });

  afterEach(() => {
    mock.stopAll();
  });

  it('saves correct base path', () => {
    assert.equal(manager.basePath, DUMMY_PATH);
  });

  describe('#saveImage', () => {
    it('calls given URL', () => {
      manager.saveImage(DUMMY_NAME, DUMMY_URL);
      assert(downloaderSpy.calledWithMatch({url: DUMMY_URL}));
    });

    it('uses correct destination', () => {
      manager.saveImage(DUMMY_NAME, DUMMY_URL);
      assert(downloaderSpy.calledWithMatch({dest: DUMMY_DEST}));
    });

    it('adds extension when needed', () => {
      manager.saveImage(DUMMY_NAME, DUMMY_URL_NO_EXT);
      assert(downloaderSpy.calledWithMatch({dest: DUMMY_DEST}));
    });
  });
});
