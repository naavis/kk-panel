var image_downloader = require('image-downloader');
var path = require('path');

module.exports = class ImageSaver {
  constructor(basePath) {
    this.basePath = basePath;
  }

  saveImage(name, url) {
    // Extension of resulting image file must be determined.
    // If URL doesn't contain extension of the image file, jpg will be used.
    let extension = url.split('.').pop();
    const acceptedExtensions = ['jpg', 'jpeg', 'png', 'gif'];
    if (acceptedExtensions.indexOf(extension) === -1) {
      extension = 'jpg';
    }

    const dest = path.join(this.basePath, name + '-latest.' + extension);
    let options = {
      url: url,
      dest: dest,
      done: function(err, filename, image) {
        if (err) {
          throw err;
        }
      }
    };
    image_downloader(options);
  }

  // TODO: Check if remote file has been modified after saving the previous one.
  // If not, no need to download.
};
