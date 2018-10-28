# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## 1.7.0 - 2018-10-28
## Added
- Add darkness graph to panel

## Changed
- Remove Kumpula from panel for now

## 1.6.1 - 2017-12-22
### Fixed
- Fix unhandled error when receiving HTTP 403 from a server.

## 1.6.0 - 2017-11-07
### Changed
- Change Tampere North URL

## 1.5.0 - 2017-11-01
### Added
- Add Tähtikallio allsky camera

## 1.4.0 - 2017-10-29
### Added
- Add Hankasalmi Allsky camera

## Changed
- Replace Tampere NE with Tampere North
- Change Tampere Allsky URL

## 1.3.0 - 2017-10-29
### Changed
- Errors during job execution get logged instead of crashing whole program

### Fixed
- Add www to Tampereen Ursa website URL
- Fix socket.io path after moving to reflect new server

## 1.2.3 - 2017-10-29
### Fixed
- Fix invalid file extension for FMI Testbed image

## 1.2.2 - 2017-10-29
### Fixed
- Fix changed Kumpula camera URL
- Fix changed FMI Testbed camera URL

## 1.2.1 - 2017-03-13
### Changed
- Change text for unavailable images

## 1.2.0 - 2017-01-12
### Fixed
- Cache problem causing images not to reload on production server

## 1.1.0 - 2017-01-12
### Added
- Prevent caching of images in the browser
- Log Socket.io messages in browser

### Changed
- Adjust job schedules for Kumpula and Tampere cameras

## 1.0.0 - 2017-01-07
### Added
- Change log file

### Changed
- Bump version number to 1.0.0
