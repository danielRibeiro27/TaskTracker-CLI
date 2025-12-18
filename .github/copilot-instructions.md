# TaskTracker-CLI Coding Instructions

## Project Overview
**TaskTracker-CLI** is a C# .NET 8.0 console application for tracking tasks and managing to-do lists. It uses the `System.CommandLine` library for argument parsing and command routing.

## Architecture

### Command Pattern (Primary Structure)
The app implements a command pattern with a centralized router:

- **`Program.cs`** - Entry point that creates a `RootCommand` and delegates to `CommandsManager`
- **`commands/CommandsManager.cs`** - Registers all subcommands (list, create, update, delete) with the root command
- **`commands/{CreateCommand,ListCommand,UpdateCommand,DeleteCommand}.cs`** - Individual command implementations

Each command class has a single public method (e.g., `Create()`, `List()`) that contains the implementation.

### Key Pattern: System.CommandLine Integration
Commands are registered via `CommandsManager.RegisterSubCommands()`:
```csharp
Command createCommand = new("create", "Creates a new task");
createCommand.SetAction(parseResult => {
    CreateCommand create = new();
    create.Create();
    return 0;
});
rootCommand.Subcommands.Add(createCommand);
```

**New commands should follow this pattern:**
1. Create a class in `src/commands/` with a descriptive method
2. Register it in `CommandsManager.RegisterSubCommands()` with description
3. Return exit code 0 on success, non-zero on failure

## Build & Run

### Build (Debug)
```bash
cd src
dotnet build
# Output: bin/Debug/net8.0/task
```

### Run Commands
```bash
# After building, commands execute as: ./bin/Debug/net8.0/task <command>
./bin/Debug/net8.0/task list      # List all tasks
./bin/Debug/net8.0/task create    # Create new task
./bin/Debug/net8.0/task update    # Update a task
./bin/Debug/net8.0/task delete    # Delete a task
```

### Current State
All command implementations are stubs with placeholder `Console.WriteLine()` statements. The branch `3-implement-cli-argument-parser-command-router` indicates CLI routing is in progress.

## Conventions & Patterns

- **Namespace**: All commands use `namespace TaskTracker.Commands`
- **Language**: Portuguese comments in command files (e.g., "Implementação do comando" placeholders)
- **Output**: Use `Console.WriteLine()` for user-facing messages
- **Exit Codes**: Return 0 from command methods (int Main handles conversion)

## File Locations

| File | Purpose |
|------|---------|
| `src/Program.cs` | CLI entry point and routing setup |
| `src/commands/CommandsManager.cs` | Central command registration hub |
| `src/commands/*Command.cs` | Individual command implementations |
| `src/TaskTracker-CLI.csproj` | Project config (targets net8.0, RootNamespace: task) |

## Dependencies

- **System.CommandLine v2.0.0** - Argument parsing and command routing
- **.NET 8.0** - Runtime

## Notes for Implementation

1. Commands are currently stubs—focus on implementing the actual task tracking logic (likely requires data persistence)
2. The `ParseResult` parameter in command actions can be extended to accept arguments (currently unused)
3. Consider adding data models and persistence layer (not yet present in the codebase)
4. The assembly name is `task` (see .csproj), so the executable is named `task`
