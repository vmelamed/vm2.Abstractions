# Changelog

## v1.0.0-preview.3 - 2026-06-11

### Internal

- update cache-pattern description for clarity

## v1.0.0-preview.2 - 2026-06-11

### Added

- add max generation collection thresholds to CI environment variables [skip ci]

### Fixed

- disable AoT, refactor Directory.Build.props
- streamline the dev. environment for multi-OS/multi-IDE and for consistent configuration of AI [skip ci]
- update commit prefix for git-cliff to include 'tests' and adjust documentation
- remove trailing newline from file header template; add file headers
- remove trailing backticks from CLI commands in README
- correct variable naming and update package versions in lock files
- update conventions and add ExcludeFromCodeCoverage attribute to ITenanted interface
- add OutputType to Abstractions.Tests project file

### Internal

- Bump the minor-and-patch group with 1 update
- Bump the minor-and-patch group with 1 update
- rename test/ to tests/
- fix the SPDX file header
- tenant management interfaces and implementations
- update vm2.TestUtilities to version 2.1.0
- update package versions in packages.lock.json to 10.0.9 and 10.0.300
- update vm2.TestUtilities to version 2.1.1
- changed comments and UI for clarity [skip ci]
- add packages.lock.json with updated dependencies

## v1.0.0-preview.1 - 2026-05-21

### Internal

- initial scaffold
- Bump the minor-and-patch group with 14 updates
- update dependencies and improve project conventions
- update copilot instructions package name to vm2.Abstractions
- sync with diff-shared.sh
- update vm2.TestUtilities to version 1.5.1
- fix typo in conventions for merge or copy action description
- improve wording in CI warning message and conventions documentation
- update vm2.TestUtilities to version 1.5.1 and remove unused dependencies
- update changelog for v1.0.0-preview.1 [skip ci]

## v1.0.0-preview.1 - 2026-05-21

### Internal

- initial scaffold
- Bump the minor-and-patch group with 14 updates
- update dependencies and improve project conventions
- update copilot instructions package name to vm2.Abstractions
- sync with diff-shared.sh
- update vm2.TestUtilities to version 1.5.1
- fix typo in conventions for merge or copy action description
- improve wording in CI warning message and conventions documentation
- update vm2.TestUtilities to version 1.5.1 and remove unused dependencies

All notable changes to this project will be documented in this file.

## [0.0.0] - YYYY-MM-DD

- Initial scaffold created from vm2pkg template.

## Usage Notes

> [!TIP] Be disciplined with your commit messages and let git-cliff do the work of updating this file.
>
> **Added:**
>
> - add new features
> - commit prefix for git-cliff: `feat:`
>
> **Changed:**
>
> - add behavior changes
> - commit prefix for git-cliff: `refactor:`
>
> **Fixed:**
>
> - add bug fixes
> - commit prefix for git-cliff: `fix:`
>
> **Performance**
>
> - add performance improvements
> - commit prefix for git-cliff: `perf:`
>
> **Security**
>
> - add security-related changes
> - commit prefix for git-cliff: `security:`
>
> **Remove or Revert**
>
> - add removed/obsolete items
> - commit prefix for git-cliff: `revert:` or `remove:`
>
> **Internal**
>
> - add internal changes
> - commit prefix for git-cliff: `refactor:`, `doc:`, `docs:`, `style:`, `test:`, `chore:`, `ci:`, `build:`
>

## References

This format follows:

- [Keep a Changelog](https://keepachangelog.com/en/1.1.0/)
- [Semantic Versioning](https://semver.org/)
- Version numbers are produced by [MinVer](./ReleaseProcess.md) from Git tags.
