using System.Linq;
using TMPro;
using UnityEngine;

public class DebugConsole : MonoBehaviour, IGameConsole
{
    [Header("UI")]
    [SerializeField] private GameObject consolePanel;
    [SerializeField] private TMP_Text logText;
    [SerializeField] private TMP_Text suggestionText;
    [SerializeField] private TMP_InputField inputField;

    [Header("Refs")]
    [SerializeField] private WeaponDatabase weaponDatabase;

    //bool consoleOpen = false;

    private ConsoleCommandRegistry _registry;
    private bool _consoleOpen;
    private void Awake()
    {
        _registry = new ConsoleCommandRegistry();
        _registry.Register(new HelpCommand(_registry));
        _registry.Register(new PlayerWeaponCommand(weaponDatabase));
        _registry.Register(new PlayerAmmoCommand());

        ToggleConsole(false);

        GameServices.Input.Actions.Debug.ToggleConsole.performed += ctx =>
        {
            _consoleOpen = !_consoleOpen;
            ToggleConsole(_consoleOpen);
        };
        GameServices.Input.Actions.Debug.AutoComplete.performed += ctx => HandleAutocomplete();
        inputField.onSubmit.AddListener(OnSubmit);
    }
    private void OnDestroy()
    {
        inputField.onSubmit.RemoveListener(OnSubmit);
        
        GameServices.Input.Actions.Debug.ToggleConsole.performed -= ctx =>
        {
            _consoleOpen = !_consoleOpen;
            ToggleConsole(_consoleOpen);
        };
        GameServices.Input.Actions.Debug.AutoComplete.performed -= ctx => HandleAutocomplete();
    }
    public void Log(string message, ConsoleLogType type = ConsoleLogType.Info)
    {
        Color color = type switch
        {
            ConsoleLogType.Info => Color.white,
            ConsoleLogType.Warning => Color.yellow,
            ConsoleLogType.Error => Color.red,
            _ => Color.white
        };
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        logText.text += $"\n<color=#{hexColor}>[{type}] {message}</color>";
    }
    private void OnSubmit(string text) 
    {
        _registry.TryExecute(text, this);
        inputField.text = string.Empty;
        inputField.Select();
        inputField.ActivateInputField();
    }
    
    private void ToggleConsole(bool open) 
    {
        consolePanel.SetActive(open);
        _consoleOpen = consolePanel.activeInHierarchy;

        if (_consoleOpen) 
        {
            inputField.Select();
            inputField.ActivateInputField();
        }
        GameServices.Input.TogglePlayerInput(!_consoleOpen);
    }
    private void HandleAutocomplete() 
    {
        string text = inputField.text;
        if (string.IsNullOrWhiteSpace(text))
            return;
        string[] parts = text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return;

        string cmdToken = parts[0];

        if(parts.Length == 1) 
        {
            var matches = _registry.FindByPrefix(cmdToken).ToList();

            if(matches.Count == 0) 
            {                
                return;
            }

            if(matches.Count == 1) 
            {
                inputField.text = matches[0].Name + " ";
                inputField.caretPosition = inputField.text.Length;
                inputField.selectionAnchorPosition = inputField.caretPosition;
                inputField.selectionFocusPosition = inputField.caretPosition;
                inputField.ActivateInputField();
            }
            else 
            {
                Log("Commands: " + string.Join(", ", matches.Select(m => m.Name)));
            }
            return;
        }

        var cmd = _registry.GetExact(cmdToken);
        if(cmd == null) 
        {
            Log($"Unknown command '{cmdToken}'", ConsoleLogType.Error);
            return;
        }
        string[] args = parts.Skip(1).ToArray();

        var rawSuggestions = cmd.GetSuggestions(args).ToList();
        if (rawSuggestions.Count == 0)
            return;

        string lastArg = args[^1];
        var matchesForLast = rawSuggestions.
            Where(s => s.StartsWith
            (lastArg, System.StringComparison.OrdinalIgnoreCase)).
            ToList();
        
        if (matchesForLast.Count == 0)
        {
            Log("Options:");
            foreach (var s in rawSuggestions)
            {
                string full = BindFullSuggestion(cmdToken, args, s);
                Log(full);
            }
            return;
        }

        if (matchesForLast.Count == 1)
        {
            string completed = matchesForLast[0];
            string newInput = BindFullSuggestion(cmdToken, args, completed);

            inputField.text = newInput + " ";
            inputField.caretPosition = inputField.text.Length;
            inputField.selectionAnchorPosition = inputField.caretPosition;
            inputField.selectionFocusPosition = inputField.caretPosition;
            inputField.ActivateInputField();
            return;
        }

        
        Log("Options:");
        foreach (var s in matchesForLast)
        {
            string full = BindFullSuggestion(cmdToken, args, s);
            Log(full);
        }

    }
    private string BindFullSuggestion(string cmdName, string[] args, string lastReplacement) 
    {
        if (args.Length <= 1)
            return $"{cmdName} {lastReplacement}";

        var beforeLast = args.Take(args.Length - 1);
        return cmdName + " " + string.Join(" ", beforeLast) + " " + lastReplacement;
    } 
    private void LogInputField(string s) 
    {
        suggestionText.text = s;
    }
    /*
    private void ToggleConsole(bool open) 
    {
        inputField.Select();
        consolePanel.SetActive(open);
        consoleOpen = consolePanel.activeInHierarchy;
        GameServices.Input.TogglePlayerInput(consoleOpen);
    }
    private void LogMessage(string msg, Color? color = null) 
    {
        Color finalColor = color ?? Color.white;

        string hexColor = ColorUtility.ToHtmlStringRGB(finalColor);
        logText.text += $"\n<color=#{hexColor}>{msg}</color>";
        inputField.text = "";
    }
    private void ParseCommand(string command)
    {
        if (string.IsNullOrEmpty(command))
            LogMessage("Invalid", Color.red);
        string[] parts = command.Split(' ');
        string cmd = parts[0].Trim();
        string[] args = parts.Length > 1 ? parts[1..] : new string[0];

        if (cmd.ToLower().StartsWith("player")) 
        {
            HandlePlayerCommand(cmd, args);
        }
        inputField.Select();

    }
    #region PlayerCommand
    private void HandlePlayerCommand(string cmd, string[] args)
    {
        if (string.IsNullOrEmpty(args[0]))
        {
            LogMessage("Arg[0] Invalid", Color.red);
            return;
        }
        switch (args[0].ToLower())
        {
            case "weapon":
                HandleWeaponCommand(args);
                break;
            case "ammo":
                HandleAmmoCommand(args);
                break;
            case "teleport":
    
                break;
        }
    }
    private void HandleTeleportCommand(string[] args) 
    {
        
    }
    private void HandleAmmoCommand(string[] args) 
    {
        if(args.Length < 2) 
        {
            LogMessage("Missing Ammo Type", Color.red);
            return;
        }
        if(args.Length < 3) 
        {
            LogMessage("Missing Ammo Amount", Color.red);
            return;
        }
        int.TryParse(args[2], out int amount);

        switch (args[1].ToLower()) 
        {            
            case "9mm":
                GameServices.WeaponController.AddAmmo(AmmoType.A_9mm, amount);                
                break;
            case "357":
                GameServices.WeaponController.AddAmmo(AmmoType.A_357, amount);
                break;
            default:
                LogMessage($"Ammo Type: {args[1]} Is Invalid", Color.red);
                break;
                
                
        }
    }
    private void HandleWeaponCommand(string[] args)
    {

        if (string.IsNullOrEmpty(args[1]))
        {
            LogMessage("Arg[1] Invalid", Color.red);
            return;
        }
        if (string.IsNullOrEmpty(args[2]))
        {
            LogMessage("Arg[2] Invalid", Color.red);
            return;
        }

        WeaponData weaponData = null;
        for (int i = 0; i < weaponDatabase.AllWeapons.Count; i++)
        {
            if (weaponDatabase.AllWeapons[i].name.ToLower() == args[2].ToLower())
            {
                weaponData = weaponDatabase.AllWeapons[i];
            }
        }
        if (weaponData == null)
        {
            LogMessage("Invalid Weapon", Color.yellow);
            return;
        }
        switch (args[1].ToLower())
        {
            case "add":
                GameServices.WeaponController.AddWeapon(weaponData);
                LogMessage($"{weaponData.weaponName} added", Color.green);
                break;
            case "remove":

                break;            
            default:
                LogMessage("Invalid Arg[1]", Color.red);
                break;
        }
    }
    
    #endregion    
    */
}
