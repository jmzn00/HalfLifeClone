using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;

    private void Awake()
    {
        WeaponController.OnAmmoChanged += UpdateAmmoDisplay;
    }

    private void UpdateAmmoDisplay(int mag, int reserve) 
    {
        ammoText.text = $"{mag} / {reserve}";
    }
}
