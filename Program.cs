using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.Json;

namespace TaskTracker;

class Program
{
    static int Main(string[] args)
    {
        // Handle help command directly
        if (args.Length == 0 || args[0] == "help" || args[0] == "--help" || args[0] == "-h" || args[0] == "-?")
        {
            ShowHelp();
            return 0;
        }

        var rootCommand = new RootCommand("TaskTracker - A CLI app to track tasks and manage your to-do list");

        // Add command
        var addCommand = new Command("add", "Add a new task");
        var addDescriptionArg = new Argument<string>("description") 
        {
            Description = "Task description"
        };
        addCommand.Arguments.Add(addDescriptionArg);
        rootCommand.Subcommands.Add(addCommand);

        // Update command
        var updateCommand = new Command("update", "Update an existing task");
        var updateIdArg = new Argument<int>("id") 
        {
            Description = "Task ID"
        };
        var updateDescriptionArg = new Argument<string>("description") 
        {
            Description = "New task description"
        };
        updateCommand.Arguments.Add(updateIdArg);
        updateCommand.Arguments.Add(updateDescriptionArg);
        rootCommand.Subcommands.Add(updateCommand);

        // Delete command
        var deleteCommand = new Command("delete", "Delete a task");
        var deleteIdArg = new Argument<int>("id") 
        {
            Description = "Task ID"
        };
        deleteCommand.Arguments.Add(deleteIdArg);
        rootCommand.Subcommands.Add(deleteCommand);

        // Mark-in-progress command
        var markInProgressCommand = new Command("mark-in-progress", "Mark a task as in progress");
        var markInProgressIdArg = new Argument<int>("id") 
        {
            Description = "Task ID"
        };
        markInProgressCommand.Arguments.Add(markInProgressIdArg);
        rootCommand.Subcommands.Add(markInProgressCommand);

        // Mark-done command
        var markDoneCommand = new Command("mark-done", "Mark a task as done");
        var markDoneIdArg = new Argument<int>("id") 
        {
            Description = "Task ID"
        };
        markDoneCommand.Arguments.Add(markDoneIdArg);
        rootCommand.Subcommands.Add(markDoneCommand);

        // List command
        var listCommand = new Command("list", "List all tasks or filter by status");
        var statusOption = new Option<string?>("--status")
        {
            Description = "Filter by status (done, todo, in-progress)"
        };
        listCommand.Options.Add(statusOption);
        rootCommand.Subcommands.Add(listCommand);

        ParseResult parseResult = rootCommand.Parse(args);
        
        if (parseResult.Errors.Count > 0)
        {
            foreach (ParseError parseError in parseResult.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
            }
            Console.WriteLine();
            ShowHelp();
            return 1;
        }

        // Handle commands
        var commandResult = parseResult.CommandResult;
        
        if (commandResult.Command.Name == "add")
        {
            string description = parseResult.GetValue(addDescriptionArg) ?? string.Empty;
            AddTask(description);
            return 0;
        }
        else if (commandResult.Command.Name == "update")
        {
            int id = parseResult.GetValue(updateIdArg);
            string description = parseResult.GetValue(updateDescriptionArg) ?? string.Empty;
            UpdateTask(id, description);
            return 0;
        }
        else if (commandResult.Command.Name == "delete")
        {
            int id = parseResult.GetValue(deleteIdArg);
            DeleteTask(id);
            return 0;
        }
        else if (commandResult.Command.Name == "mark-in-progress")
        {
            int id = parseResult.GetValue(markInProgressIdArg);
            MarkTaskInProgress(id);
            return 0;
        }
        else if (commandResult.Command.Name == "mark-done")
        {
            int id = parseResult.GetValue(markDoneIdArg);
            MarkTaskDone(id);
            return 0;
        }
        else if (commandResult.Command.Name == "list")
        {
            string? status = parseResult.GetValue(statusOption);
            ListTasks(status);
            return 0;
        }

        // If no command matched, show help
        ShowHelp();
        return 0;
    }

    static void ShowHelp()
    {
        Console.WriteLine("TaskTracker - A CLI app to track tasks and manage your to-do list");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  task <command> [arguments] [options]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  add <description>              Add a new task");
        Console.WriteLine("  update <id> <description>      Update an existing task");
        Console.WriteLine("  delete <id>                    Delete a task");
        Console.WriteLine("  mark-in-progress <id>          Mark a task as in progress");
        Console.WriteLine("  mark-done <id>                 Mark a task as done");
        Console.WriteLine("  list [--status <status>]       List all tasks or filter by status");
        Console.WriteLine("  help                           Show this help message");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --help, -h                     Show help information");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  task add \"Buy groceries\"");
        Console.WriteLine("  task list");
        Console.WriteLine("  task list --status done");
        Console.WriteLine("  task mark-in-progress 1");
        Console.WriteLine("  task mark-done 1");
        Console.WriteLine("  task update 1 \"Buy groceries and cook dinner\"");
        Console.WriteLine("  task delete 1");
    }

    static string GetTaskFilePath()
    {
        return Path.Combine(Environment.CurrentDirectory, "tasks.json");
    }

    static List<Task> LoadTasks()
    {
        string filePath = GetTaskFilePath();
        if (!File.Exists(filePath))
        {
            return new List<Task>();
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
    }

    static void SaveTasks(List<Task> tasks)
    {
        string filePath = GetTaskFilePath();
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    static void AddTask(string description)
    {
        var tasks = LoadTasks();
        int newId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
        var newTask = new Task
        {
            Id = newId,
            Description = description,
            Status = "todo",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        tasks.Add(newTask);
        SaveTasks(tasks);
        Console.WriteLine($"Task added successfully (ID: {newId})");
    }

    static void UpdateTask(int id, string description)
    {
        var tasks = LoadTasks();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            Console.Error.WriteLine($"Task with ID {id} not found");
            return;
        }
        task.Description = description;
        task.UpdatedAt = DateTime.UtcNow;
        SaveTasks(tasks);
        Console.WriteLine($"Task {id} updated successfully");
    }

    static void DeleteTask(int id)
    {
        var tasks = LoadTasks();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            Console.Error.WriteLine($"Task with ID {id} not found");
            return;
        }
        tasks.Remove(task);
        SaveTasks(tasks);
        Console.WriteLine($"Task {id} deleted successfully");
    }

    static void MarkTaskInProgress(int id)
    {
        var tasks = LoadTasks();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            Console.Error.WriteLine($"Task with ID {id} not found");
            return;
        }
        task.Status = "in-progress";
        task.UpdatedAt = DateTime.UtcNow;
        SaveTasks(tasks);
        Console.WriteLine($"Task {id} marked as in progress");
    }

    static void MarkTaskDone(int id)
    {
        var tasks = LoadTasks();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            Console.Error.WriteLine($"Task with ID {id} not found");
            return;
        }
        task.Status = "done";
        task.UpdatedAt = DateTime.UtcNow;
        SaveTasks(tasks);
        Console.WriteLine($"Task {id} marked as done");
    }

    static void ListTasks(string? status)
    {
        var tasks = LoadTasks();
        if (status != null)
        {
            tasks = tasks.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }

        foreach (var task in tasks)
        {
            Console.WriteLine($"[{task.Id}] {task.Description} - Status: {task.Status}");
        }
    }
}

class Task
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "todo";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}