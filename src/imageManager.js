var image_downloader = require('image-downloader');
var path = require('path');
var fs = require('fs');

module.exports = class ImageManager {
  constructor(basePath) {
    this.basePath = path.normalize(basePath);

    // Create directory for images, if it does not exist.
    try {
      fs.statSync(this.basePath);
    } catch (error) {
      if (error.code !== 'ENOENT') {
        throw error;
      }
      fs.mkdirSync(this.basePath);
    }
  }

  saveImage(name, url, callback) {
    // Extension of resulting image file must be determined.
    // If URL doesn't contain extension of the image file, jpg will be used.
    let extension = url.split('.').pop();
    const acceptedExtensions = ['jpg', 'jpeg', 'png', 'gif'];
    if (acceptedExtensions.indexOf(extension) === -1) {
      extension = 'jpg';
    }

    let doneFn = function(err, filename, image) {
      if (err) {
        throw err;
      }
    };

    const dest = path.join(this.basePath, name + '-latest.' + extension);
    let options = {
      url: url,
      dest: dest,
      done: (callback || doneFn)
    };
    image_downloader(options);
  }

  // TODO: Check if remote file has been modified after saving the previous one.
  // If not, no need to download.
};
