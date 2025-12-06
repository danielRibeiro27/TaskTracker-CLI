using System.CommandLine;

namespace TaskTracker.Commands
{
    public class CommandsManager
    {
        public void RegisterSubCommands(RootCommand rootCommand)
        {
            Command listCommand = new("list", "Lists all tasks");
            listCommand.SetAction(parseResult =>
            {
                ListCommand list = new();
                list.List();
                return 0;
            });
            rootCommand.Subcommands.Add(listCommand);

            Command updateCommand = new("update", "Updates a task");
            updateCommand.SetAction(parseResult =>
            {
                UpdateCommand update = new();
                update.Update();
                return 0;
            });
            rootCommand.Subcommands.Add(updateCommand);

            Command deleteCommand = new("delete", "Deletes a task");
            deleteCommand.SetAction(parseResult =>
            {
                DeleteCommand delete = new();
                delete.Delete();
                return 0;
            });
            rootCommand.Subcommands.Add(deleteCommand);

            Command createCommand = new("create", "Creates a new task");
            createCommand.SetAction(parseResult =>
            {
                CreateCommand create = new();
                create.Create();
                return 0;
            });
            rootCommand.Subcommands.Add(createCommand);
        }
    }
}