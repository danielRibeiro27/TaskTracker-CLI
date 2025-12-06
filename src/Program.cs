using System.CommandLine;
using TaskTracker.Commands;

namespace TaskTracker;

class Program
{
    static int Main(string[] args)
    {
        RootCommand rootCommand = new("Task Tracker");
        CommandsManager manager = new();
        manager.RegisterSubCommands(rootCommand);

        return rootCommand.Parse(args).Invoke();
    }
}