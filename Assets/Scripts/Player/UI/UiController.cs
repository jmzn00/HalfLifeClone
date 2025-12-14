using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;

    [Header("WeaponUi")]
    [SerializeField] private WeaponColumnUi[] columns;
    [SerializeField] private WeaponSlotUi weaponSlotPrefab;

    [Header("HealthUi")]
    [SerializeField] private Slider healthSlider;

    [Header("Message Ui")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float messageDuration;

    [Header("GameOver Ui")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private Button restartGameButton;
    [SerializeField] private Button quitGameButton;

    private readonly Dictionary<WeaponData, WeaponSlotUi> slots = new();

    private void Awake()
    {
        WeaponController.OnAmmoChanged += UpdateAmmoDisplay;
        WeaponController.OnWeaponChanged += RebuildWeaponUi;
        PlayerHealth.OnHealthChanged += UpdateHealth;
        GameServices.GameManager.OnGameEnded += GameEnded;

        restartGameButton.onClick.AddListener(() => 
        {
            GameServices.GameManager.LoadScene(1);
        });
        quitGameButton.onClick.AddListener(() =>
        {
            GameServices.GameManager.LoadScene(0);
        });
    }
    private void OnDestroy()
    {
        WeaponController.OnAmmoChanged -= UpdateAmmoDisplay;
        WeaponController.OnWeaponChanged -= RebuildWeaponUi;
        PlayerHealth.OnHealthChanged -= UpdateHealth;
    }
    private void UpdateHealth(float value) 
    {
        healthSlider.value = value;
    }
    private void RebuildWeaponUi(List<WeaponData> wl, WeaponData c) 
    {
        if(slots.Count != wl.Count || slots.Count == 0) 
        {
            ClearAllSlots();
            BuildSlots(wl);
        }
        foreach(var kvp in slots) 
        {
            bool isSelected = kvp.Key == c;
            kvp.Value.SetSelected(isSelected);
        }
        foreach(var col in columns) 
        {
            if(col != null)
                col.RefeshWidth();
        }
    }
    private void ClearAllSlots() 
    {
        foreach(var kvp in slots) 
        {
            if(kvp.Value != null)
                Destroy(kvp.Value.gameObject);
        }
        slots.Clear();
    }
    private void BuildSlots(List<WeaponData> wl) 
    {
        foreach(var w in wl) 
        {
            int colIndex = (int)w.weaponColumn - 1;
            if (colIndex < 0 || colIndex >= columns.Length)
            {
                Debug.LogWarning($"Weapon {w.weaponName} has invalid column {w.weaponColumn}");
                continue;
            }

            var columnUi = columns[colIndex];
            if(columnUi == null) 
            {
                Debug.LogWarning($"Column UI at index {colIndex} is null for weapon {w.weaponName}");
                continue;
            }

            var slot = Instantiate(weaponSlotPrefab, columnUi.transform);
            slot.Init(w);
            slots[w] = slot;
        }   
        foreach(var col in columns) 
        {
            if(col != null)
                col.RefeshWidth();
        }
    }
    private void UpdateAmmoDisplay(WeaponRuntime wr) 
    {
        if(wr.weaponData.weaponType == WeaponType.Melee)
            ammoText.gameObject.SetActive(false);
        else 
        {
            ammoText.gameObject.SetActive(true);
            ammoText.text = $"{wr.ammoInClip} / {wr.ammoInReserve}";
        }        
    }
    public void SendUiMessage(string msg) 
    {
        StartCoroutine(IMessage(msg));
    }
    IEnumerator IMessage(string msg) 
    {
        messageText.text = msg;
        yield return new WaitForSeconds(messageDuration);
        messageText.text = "";

    }
    private void GameEnded(bool end, bool won) 
    {
        if (won)
        {
            gameWonPanel.SetActive(true);
        }
        else 
        {
            gameOverPanel.SetActive(end);
        }            
    }
}
