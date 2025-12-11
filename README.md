# TaskTracker-CLI

CLI app to track tasks and manage your to-do list.

## Features

- Add new tasks
- Update existing tasks
- Delete tasks
- Mark tasks as in progress
- Mark tasks as done
- List all tasks or filter by status

## Installation

1. Clone this repository
2. Build the project:
   ```bash
   dotnet build
   ```

## Usage

```bash
task <command> [arguments] [options]
```

### Commands

- `task add <description>` - Add a new task
- `task update <id> <description>` - Update an existing task
- `task delete <id>` - Delete a task
- `task mark-in-progress <id>` - Mark a task as in progress
- `task mark-done <id>` - Mark a task as done
- `task list [--status <status>]` - List all tasks or filter by status
- `task help` - Show help message

### Options

- `--help, -h` - Show help information

### Examples

```bash
# Add a new task
task add "Buy groceries"

# List all tasks
task list

# List tasks by status
task list --status done
task list --status todo
task list --status in-progress

# Mark a task as in progress
task mark-in-progress 1

# Mark a task as done
task mark-done 1

# Update a task
task update 1 "Buy groceries and cook dinner"

# Delete a task
task delete 1

# Show help
task help
task --help
task -h
```

## Task Storage

Tasks are stored in a `tasks.json` file in the current working directory.
