# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [15.0.2] - 2022-07-01
### Changed
 - Updated Crestron SDK to 2.18.96

## [15.0.1] - 2021-08-03
### Changed
 - IcdPhysicalAddress - Allow '.' as a seperator
 - IcdPhyiscalAddress - Override equals(object obj) method

## [15.0.0] - 2021-05-14
### Added
 - Add AdapterNetworkDeviceInfoCollection so changes in network adapters are better picked up by telemetry
 - Added IcdPhysicalAddress for standardizing MAC addresses

### Changed
 - Added IPoint<T>:IPoint, AbstractPoint implements IPoint<T>
 - IDeviceInfo now implements IConsoleNode

## [14.0.1] - 2021-02-04
### Changed
 - Device online state telemetry is debounced

## [14.0.0] - 2021-01-14
### Added
 - Added RoomCritical configuration item to devices for specifying that a device is critical to a rooms operation

### Changed
 - Added OnRequestResync event to ISimplDeviceBase (removed from ISimplOriginator)
 - Added non-generic GetOrAddAdapter to NetworkDeviceInfo
 
### Removed
 - Removed Reboot info/methods from MonitoredDeviceInfo
 - Removed Model and SerialNumber properties from AbstractDevice
 - Removed unused DeviceTelemetryNames

## [13.0.0] - 2020-09-24
### Changed
 - Fixed a bug where default power activities were not being initialized
 - Changed power control namespaces

## [12.0.1] - 2020-08-13
### Changed
 - Telemetry namespace change

## [12.0.0] - 2020-07-14
### Added
 - Added MonitoredDeviceInfo and ConfiguredDeviceInfo property to IDevice and implementations
 - Added DeviceInfo enumerable property to IDevice, used for Telemetry

### Changed
 - Configured Make/Model/Serial/PurchaseDate in IDeviceSettings implementations now just access ConfiguredDeviceInfo
 - AbstractDevice Model and SerialNumber properties now just access MonitoredDeviceInfo
 - Simplified external telemetry providers

## Removed
 - Removed Configured Make/Model/Serial/PurchaseDate from IDevice and all implemetations
 - Removed Make/Model from IDevice and all unused implementaions

## [11.0.0] - 2020-06-18
### Added
 - Added MockDevice interfaces/classes to allow Mock devices to be set online/offline from console and be configured to start offline from settings
 - Added Manufacturer, Model, Serial Number and Purchase date to device and device settings
 - Controls have UUIDs
 - Devices push reported Manufacturer, Model, Serial Number, etc to telemetry
 - Devices push their list of control ids to telemetry

### Changed
 - Using new logging context
 - Controls moved from IDeviceBase to IDevice
 - Added missing proxy intialization step to simpl shims
 - Fixed JSON deserialization for PowerDeviceControlPowerStateEventData
 - Controls are instantiated on ApplySettings and disposed on ClearSettings

## [10.0.1] - 2020-11-16
### Changed
 - Fixed issue where always-online devices would never report controls available.

## [10.0.0] - 2020-03-20
### Added
 - Added struct for representing Windows device paths
 - Added IWindowsDevice interface for devices with a windows device path
 - Added events to DeviceControlsCollection that are raised when controls are added and removed

### Changed
 - Points wrap an underlying control and track control changes, such as in the case of DSP config reloads
 - Exposed Default values, exposed parsing and Device ID setting & improved comparison/equality for WindowsDevicePathInfo struct

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
