using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

public class PlayerWeaponCommand : IConsoleCommand
{
    private readonly WeaponDatabase _weaponDatabase;
    public PlayerWeaponCommand(WeaponDatabase db)
    {
        _weaponDatabase = db;
    }

    public string Name => "player.weapon";
    public string Description => "add or remove weapons from the player";
    public string Usage => "player.weapon <add/remove> <weaponName/all>";

    public IEnumerable<string> GetSuggestions(string[] args) 
    {
        if(args.Length == 1) 
        {
            return new string[] { "add", "remove" }
                .Where(a => a.StartsWith(args[0], System.StringComparison.OrdinalIgnoreCase));
        }
        else if(args.Length == 2) 
        {
            var weaponNames = _weaponDatabase.AllWeapons
                .Select(w => w.weaponName)
                .Append("all");
            return weaponNames
                .Where(w => w.StartsWith(args[1], System.StringComparison.OrdinalIgnoreCase));
        }
        return null;
    }

    public void Execute(IGameConsole console, string[] args) 
    {
        if(args.Length < 2) 
        {
            console.Log($"Usage: {Usage}", ConsoleLogType.Error);
            return;
        }

        string action = args[0].ToLower();
        string weaponName = args[1].ToLower();

        if(action == "add" && weaponName == "all") 
        {
            foreach(var w in _weaponDatabase.AllWeapons) 
            {
                GameServices.Player.Weapons.AddWeapon(w);
                console.Log($"Weapon '{w.weaponName}' added.");
            }            
            return;
        }
        WeaponData wData = _weaponDatabase.AllWeapons.FirstOrDefault(w => w.weaponName.ToLower() == weaponName);
        if(wData == null) 
        {
            console.Log($"Weapon '{weaponName}' not found.", ConsoleLogType.Error);
            return;
        }
        switch (action) 
        {
            case "add":
                GameServices.Player.Weapons.AddWeapon(wData);
                console.Log($"Weapon '{wData.weaponName}' added.");
                break;
            case "remove":
                console.Log($"Remove action not yet implemented");                
                break;
            default:
                console.Log($"Unknown action '{action}'. Usage {Usage}", ConsoleLogType.Error);
                break;
        }
    }
}
