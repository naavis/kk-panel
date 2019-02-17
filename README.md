Komakallio Observatory Panel
============================
[![Build Status](https://travis-ci.org/naavis/kk-panel.svg?branch=master)](https://travis-ci.org/naavis/kk-panel)

Dashboard used to display environmental data
and all-sky camera images near Komakallio Observatory
in Kirkkonummi, Finland.

Installation
------------
Execute the following commands to run the panel:
```
npm install
npm test
npm start
```

You can also use the `Dockerfile` to build a docker image and run it in a container.
`docker-compose.yml` is written to use a bridge network called `common-network`,
so it can be served via an nginx proxy also running in a Docker container.
