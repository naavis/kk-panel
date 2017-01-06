var cron = require('node-cron');
var jobs = require('./jobs');
var logger = require('winston');

module.exports = class Scheduler {
  constructor(...args) {
    this.jobs = new Map();
    if (args.length === 0) {
      return;
    } else if (args.length === 2) {
      let panelArray = args[0];
      let imageManager = args[1];
      panelArray.forEach((entry) => {
        let currentJob = jobs[entry.id];
        this.add(entry.id, entry.schedule, currentJob, imageManager);
      });
    } else {
      throw new Error('Invalid number of arguments!');
    }
  }

  add(name, schedule, job, ...jobArguments) {
    this.jobs.set(name, cron.schedule(schedule, function() {
      logger.debug(name + ' job starting');
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
