using Mono.Cecil;
using System.Linq;
using Unity.Collections;
using UnityEngine;

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
                GameServices.WeaponController.AddWeapon(w);
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
                GameServices.WeaponController.AddWeapon(wData);
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
