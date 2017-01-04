var sinon = require('sinon');
var mock = require('mock-require');
var assert = require('assert');
var ImageManager = require('../imageManager.js');

const DUMMY_NAME = 'dummyname';
const DUMMY_PATH = 'dummypath/';
// TODO: Check if \\ versus / in DUMMY_DEST breaks a test on *nix
const DUMMY_DEST = 'dummypath\\dummyname-latest.jpg';
const DUMMY_URL = 'http://foo.bar/baz.jpg';
const DUMMY_URL_NO_EXT = 'http://foo.bar/baz';

describe('imageManager', () => {
  describe('saveImage', () => {
    var manager = {};
    var downloaderSpy = {};

    beforeEach(() => {
      downloaderSpy = sinon.spy();
      mock('image-downloader', downloaderSpy);
      ImageManager = mock.reRequire('../imageManager.js');

      manager = new ImageManager(DUMMY_PATH);
    });

    afterEach(() => {
      mock.stopAll();
    });

    it('saves correct base path', () => {
      assert(manager.basePath === DUMMY_PATH);
    });

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
