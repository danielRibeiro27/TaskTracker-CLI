using System.CommandLine;
using System.CommandLine.Parsing;
using TaskTracker.Commands;

namespace TaskTracker;

class Program
{
    static int Main(string[] args)
    {
        RootCommand rootCommand = new("Sample app for System.CommandLine");
        CommandsManager manager = new();
        manager.RegisterSubCommands(rootCommand);

        return rootCommand.Parse(args).Invoke();
    }
}