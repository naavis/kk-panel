var assert = require('assert');
var Scheduler = require('../scheduler.js');

describe('Scheduler', () => {
  var scheduler = {};
  beforeEach(() => {
    scheduler = new Scheduler();
  });

  it('is empty be default', () => {
    assert.equal(scheduler.jobs.size, 0);
  });

  it('can be called with panel config array', () => {
    let panelArray = [
      {
        "name": "Perkkaa Skycam",
        "id": "perkkaa",
        "schedule": "30 * * * * *"
      },
      {
        "name": "Metsähovi",
        "id": "metsahovi",
        "schedule": "*/3 * * * *"
      }
    ];
    scheduler = new Scheduler(panelArray, null);

    assert.equal(scheduler.jobs.size, 2);
  });

  describe('#add', () => {
    it('adds task to job list', () => {
      scheduler.add('test job', '* * * * *', () => {}, []);
      assert.notEqual(scheduler.jobs.get('test job'), undefined);
    });
    // TODO: Add tests for cases when job doesn't exist
  });
});
