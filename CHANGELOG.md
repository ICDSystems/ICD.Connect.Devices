# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

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
