# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [7.0.0] - 2019-01-10
### Changed
 - Originators and settings moved into subdirectories

## [6.6.0] - 2020-04-30
### Added
 - Added additional telemetry names for common attributes (network info, model, serial, software info)

## [6.5.0] - 2019-07-16
### Added
 - Power controls support pre-on/off delegates for ad-hoc devices (e.g. projector lifts)

## [6.4.0] - 2019-05-15
### Added
 - Added telemetry features to base devices
 
### Changed
 - Clearer exception when control lookup fails

## [6.3.2] - 2019-02-07
### Changed
 - Better error logging for bad control lookups

## [6.3.1] - 2018-10-04
### Changed
 - Fixed formatting issue with generic device controls in the console

## [6.3.0] - 2018-09-14
### Changed
 - Control collection optimizations

## [6.2.0] - 2018-06-19
### Changed
 - Simpl shim improvements

## [6.1.0] - 2018-06-04
### Added
 - Added SimplDeviceBase abstractions and interfaces

### Changed
 - Simpl wrapper/originator online state is settable

## [6.0.0] - 2018-05-24
### Changed
 - Significant namespace changes

## [5.0.0] - 2018-05-09
### Changed
 - Fixed lookup issue with control id 0
 - Changed S+ shim naming convention
 - Standardized control logging

## [4.0.0] - 2018-05-03
### Removed
 - Moved volume controls and volume repeaters into Audio project

## [3.1.0] - 2018-04-27
### Added
 - Getting a control with id 0 now returns the first available control of that type. 

## [3.0.0] - 2018-04-23
### Added
 - Adding API attributes to device interfaces
 - Adding API proxy abstractions
 - Abstractions and interfaces for building Simpl+ devices
