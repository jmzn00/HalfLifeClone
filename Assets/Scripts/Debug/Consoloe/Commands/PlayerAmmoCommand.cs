using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoCommand : IConsoleCommand
{
    public string Name => "player.ammo";
    public string Description => "gives player ammo.";
    public string Usage => "player.ammo <type> <amount>";

    public IEnumerable<string> GetSuggestions(string[] args)
    {
        if (args.Length == 1)
        {
            return new[] { "pistol", "revolver" };
        }
        return null;
    }

    public void Execute(IGameConsole console, string[] args)
    {
        if (args.Length < 2)
        {
            console.Log($"Usage: {Usage}", ConsoleLogType.Error);
            return;
        }
        string ammoType = args[0].ToLower();
        if (!int.TryParse(args[1], out int amount) || amount <= 0)
        {
            console.Log("Amount must be a positive integer.", ConsoleLogType.Error);
            return;
        }
        switch (ammoType)
        {
            case "pistol":
                GameServices.Player.Weapons.AddAmmo(AmmoType.A_9mm, amount);
                console.Log($"Added {amount} Bullet ammo to player.");
                break;
            case "revolver":
                GameServices.Player.Weapons.AddAmmo(AmmoType.A_357, amount);
                console.Log($"Added {amount} Shell ammo to player.");
                break;
            default:
                console.Log($"Unknown ammo type '{ammoType}'. Valid types are: bullet, shell, energy.", ConsoleLogType.Error);
                break;
        }
    }

}
