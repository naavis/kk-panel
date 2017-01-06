module.exports = class JobResultHandler {
  constructor(io, imageManager) {
    this.io = io;
    this.imageManager = imageManager;
  }

  submitImage(id, url) {
    var io = this.io;
    this.imageManager.saveImage(id, url, function done(err, filename, image) {
      if (err) {
        throw err;
      }
      io.emit('refresh', {id: id, url: filename});
    });
  }

  submitData(id, data) {
    // TODO: Implement data submission
  }
};
