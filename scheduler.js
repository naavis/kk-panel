var cron = require('node-cron');

module.exports = class Scheduler {
  constructor() {
    this.jobs = new Map();
  }

  add(name, schedule, job, ...jobArguments) {
    this.jobs.set(name, cron.schedule(schedule, function() {
      console.log(name + ' job starting');
      job(...jobArguments);
    }, false));
  }

  start(name) {
    this.jobs.get(name).start();
  }

  stop(name) {
    this.jobs.get(name).stop();
  }

  startAll() {
    for (var [name, job] of this.jobs) {
      job.start();
    }
  }

  stopAll() {
    for (var [name, job] of this.jobs) {
      job.stop();
    }
  }
};
