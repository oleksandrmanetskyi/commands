# GitHub Copilot Prompts

This directory contains prompt files for GitHub Copilot agents to perform repository-wide tasks.

## Available Prompts

### agent-create-copilot-instructions-blueprint-generator.prompt.md

A blueprint generator that creates comprehensive Copilot instructions for your repository.

**Purpose**: Generates a `copilot-instructions.md` file in the `.github` directory that guides GitHub Copilot to produce code consistent with your project's standards, architecture, and technology versions.

**Prerequisites**: 
- `.github/copilot/instructions/` folder should exist with generic per-file-type instructions
- If not present, execute `/agent-create-or-update-generic-instructions.prompt.md` first

**Configuration Variables**:
- `PROJECT_TYPE`: Auto-detect|.NET|Java|JavaScript|TypeScript|React|Angular|Python|Go|Multiple|Other
- `ARCHITECTURE_STYLE`: Layered|Microservices|Monolithic|Domain-Driven|Event-Driven|Serverless|Mixed
- `CODE_QUALITY_FOCUS`: Maintainability|Performance|Security|Accessibility|Testability|All
- `DOCUMENTATION_LEVEL`: Minimal|Standard|Comprehensive
- `TESTING_REQUIREMENTS`: Unit|Integration|E2E|TDD|BDD|All
- `VERSIONING`: Semantic|CalVer|Custom

**Usage**: Execute this prompt file through GitHub Copilot to generate repository-specific Copilot instructions based on actual codebase patterns.

**Output**: Creates `.github/copilot-instructions.md` with:
- Version compatibility requirements
- Architecture guidelines
- Code quality standards
- Documentation requirements
- Testing approaches
- Project-specific patterns from codebase analysis

## How to Use

1. Ensure prerequisites are met (`.github/copilot/instructions/` folder exists)
2. Execute the desired prompt file through GitHub Copilot
3. Review and customize the generated output as needed
4. The generated instructions will guide Copilot in future code generation

## Source

These prompt files are based on templates from [solarwinds-sandbox/copilot](https://github.com/solarwinds-sandbox/copilot).
