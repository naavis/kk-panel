var cron = require('node-cron');
var jobs = require('./jobs');
var logger = require('winston');

module.exports = class Scheduler {
  constructor(panelArray, ...args) {
    this.jobs = new Map();
    if (args.length === 0) {
      return;
    }

    if (panelArray === undefined || panelArray.length === 0) {
      return;
    }

    panelArray.forEach((entry) => {
      let currentJob = jobs[entry.id];
      this.add(entry.id, entry.schedule, currentJob, entry.options, ...args);
    });
  }

  add(name, schedule, job, options, ...jobArguments) {
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
