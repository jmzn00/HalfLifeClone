using System.Collections.Generic;


public interface IGameConsole
{
    void Log(string message, ConsoleLogType type = ConsoleLogType.Info);
}
public enum ConsoleLogType
{
    Info,
    Warning,
    Error
}
public interface IConsoleCommand 
{
    string Name { get; }
    string Description { get; }
    string Usage { get; }

    void Execute(IGameConsole console, string[] args);

    IEnumerable<string> GetSuggestions(string[] args);
}
