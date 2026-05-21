# Changelog

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
