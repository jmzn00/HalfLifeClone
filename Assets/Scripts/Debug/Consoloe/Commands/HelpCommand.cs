using Unity.Collections;
using UnityEngine;

public class HelpCommand : IConsoleCommand
{
    private readonly ConsoleCommandRegistry _registry;
    public HelpCommand(ConsoleCommandRegistry registry)
    {
        _registry = registry;
    }
    public string Name => "help";
    public string Description => "Lists all available console commands.";
    public string Usage => "help";
    public void Execute(IGameConsole console, string[] args)
    {
        console.Log("Available Commands:");
        foreach (var cmd in _registry.GetAllCommands())
        {
            console.Log($"{cmd.Name} - {cmd.Description} | Usage: {cmd.Usage}");
        }
    }
}
