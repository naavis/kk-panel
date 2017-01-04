var assert = require('assert');
var Scheduler = require('../scheduler.js');

describe('Scheduler', () => {
  describe('#add', () => {
    var scheduler = {};
    beforeEach(() => {
      scheduler = new Scheduler();
    });

    it('is empty be default', () => {
      assert.equal(scheduler.jobs.size, 0);
    });

    it('adds task to job list', () => {
      scheduler.add('test job', '* * * * *', () => {}, []);
      assert.notEqual(scheduler.jobs.get('test job'), undefined);
    });
    // TODO: Add tests for cases when job doesn't exist
  });
});
