# AI Agent Context Optimization

This directory contains context optimization configurations for AI agents working with the AVRO platform.

## Purpose

AI agents require structured context to navigate and work effectively within large monorepos. These configurations provide:

1. **Navigation Hints**: Quick reference to key files and directories
2. **Architectural Patterns**: Standard patterns used across the codebase
3. **Dependency Maps**: Understanding of cross-project relationships
4. **Autonomy Levels**: Which areas allow autonomous changes vs. requiring review

## Context Files

- `navigation.json` - Directory structure and key file locations
- `patterns.json` - Common architectural patterns and conventions
- `dependency-graph.json` - Project dependency relationships
- `autonomy-rules.json` - Rules for autonomous vs. supervised changes

## Usage

AI agents should load these context files at initialization to:
- Understand the overall architecture
- Navigate efficiently to relevant code
- Follow established patterns
- Know when human review is required
- Respect critical paths and safety boundaries

## Context Loading Order

1. Load `navigation.json` for initial orientation
2. Load `patterns.json` for coding standards
3. Load `dependency-graph.json` for impact analysis
4. Load `autonomy-rules.json` for safety constraints

## Updating Context

Context files should be updated when:
- New projects are added
- Architectural patterns change
- Dependency relationships evolve
- Autonomy rules are refined

Automated tooling maintains most context data, but manual review ensures accuracy.
