# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
 - Added struct for representing Windows device paths
 - Added IWindowsDevice interface for devices with a windows device path
 - Added events to DeviceControlsCollection that are raised when controls are added and removed

## [9.1.0] - 2019-10-07
### Added
 - Added point interfaces and abstractions
 - Added extension methods for getting the control for a given point
 - Added features for supporting warmup/cooldown states and durations

### Changed
 - IPowerDeviceControl.OnPowerStateChanged now includes ExpectedDuration in event

## [9.0.0] - 2019-09-16
### Added
 - Devices and controls have features for determining if controls are available

### Changed
 - Updated IPowerDeviceControls to use PowerState
 - IDeviceControl.DeviceControlInfo is now an extension method

## [8.0.0] - 2019-08-15
### Added
 - Added support for device control proxies

### Changed
 - Substantial changes to the Multi-Krang API, proxies and shims

## [7.0.0] - 2019-01-10
### Changed
 - Originators and settings moved into subdirectories

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
