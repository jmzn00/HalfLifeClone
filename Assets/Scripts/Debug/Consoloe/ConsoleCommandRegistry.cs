using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ConsoleCommandRegistry
{
    private readonly Dictionary<string, IConsoleCommand> _commands =
        new(StringComparer.OrdinalIgnoreCase);
    public void Register(IConsoleCommand command) 
    {
        _commands[command.Name] = command;
    }

    public IEnumerable<IConsoleCommand> GetAllCommands() => _commands.Values;

    public IEnumerable<IConsoleCommand> FindByPrefix(string prefix) 
    {
        if (string.IsNullOrWhiteSpace(prefix))
            return Enumerable.Empty<IConsoleCommand>();

        return _commands.Values.Where(c => c.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
    public IConsoleCommand GetExact(string name) 
    {
        _commands.TryGetValue(name, out var cmd);
        return cmd;
    }
    public bool TryExecute(string input, IGameConsole console) 
    {
        if (string.IsNullOrWhiteSpace(input)) 
        {
            console.Log("Empty Command.", ConsoleLogType.Error);
            return false;
        }

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string cmdName = parts[0];
        string[] args = parts.Skip(1).ToArray();

        if (!_commands.TryGetValue(cmdName, out var command))
        {
            console.Log($"Unknown command: {cmdName}", ConsoleLogType.Error);
            return false;
        }
        try 
        {
            command.Execute(console, args);
            return true;
        }
        catch (Exception ex) 
        {
            console.Log($"Command '{cmdName} failed: {ex.Message}", ConsoleLogType.Error);
            return false;
        }
    }

}
