var assert = require('assert');
var Scheduler = require('../src/scheduler.js');

describe('Scheduler', () => {
  var scheduler = {};
  beforeEach(() => {
    scheduler = new Scheduler([], {});
  });

  it('is empty be default', () => {
    assert.equal(scheduler.jobs.size, 0);
  });

  it('can be called with panel config array', () => {
    const dummyPanelArray = [
      {
        "name": "Perkkaa Skycam",
        "id": "perkkaa",
        "schedule": "30 * * * * *",
        "options": {
        }
      },
      {
        "name": "Metsähovi",
        "id": "metsahovi",
        "schedule": "*/3 * * * *",
        "options": {
        }
      }
    ];
    const dummyResultHandler = {};
    scheduler = new Scheduler(dummyPanelArray, dummyResultHandler);

    assert.equal(scheduler.jobs.size, 2);
  });

  describe('#add', () => {
    it('adds task to job list', () => {
      scheduler.add('test job', '* * * * *', () => { }, {}, null);
      assert.notEqual(scheduler.jobs.get('test job'), undefined);
    });
  });

  describe('#remove', () => {
    it('removes a task from job list', () => {
      scheduler.add('test job', '* * * * *', () => { }, {}, null);
      assert.notEqual(scheduler.jobs.size, 0);
      scheduler.remove('test job');
      assert.equal(scheduler.jobs.size, 0);
    });

    it('throws exception if job does not exist', () => {
      assert.throws(() => { scheduler.remove('foo'); });
    });
  });

  describe('#start', () => {
    it('throws exception if job does not exist', () => {
      assert.throws(() => { scheduler.start('foo'); });
    });
  });

  describe('#stop', () => {
    it('throws exception if job does not exist', () => {
      assert.throws(() => { scheduler.stop('foo'); });
    });
  });
});
