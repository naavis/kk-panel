var cron = require('node-cron');
var jobs = require('./jobs');
var logger = require('winston');

module.exports = class Scheduler {
  constructor(panelArray, jobResultHandler) {
    this.jobs = new Map();
    panelArray.forEach((entry) => {
      let currentJob = jobs[entry.id];
      if (currentJob === undefined) {
        throw new Error('Invalid job id!');
      }
      this.add(entry.id, entry.schedule, entry.options, currentJob, jobResultHandler);
    });
  }

  add(name, schedule, options, job, resultHandler) {
    this.jobs.set(name, cron.schedule(schedule, function() {
      logger.debug(name + ' job starting');
      try {
        job(resultHandler);
      } catch (err) {
        logger.error(name + ': error! ' + err);
      }
    }, false));
  }

  remove(name) {
    if (!this.jobs.delete(name)) {
      throw new Error(`Job ${name} does not exist!`);
    }
  }

  start(name) {
    let job = this.jobs.get(name);
    if (job === undefined) {
      throw new Error(`Job ${name} does not exist!`);
    }
    job.start();
  }

  stop(name) {
    let job = this.jobs.get(name);
    if (job === undefined) {
      throw new Error(`Job ${name} does not exist!`);
    }
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
