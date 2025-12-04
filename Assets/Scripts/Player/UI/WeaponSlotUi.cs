using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUi : MonoBehaviour
{
    [SerializeField] private RectTransform rootRect;
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    public LayoutElement layoutElement;

    public Vector3 normalSize = new Vector3(64, 64);
    public Vector3 selectedSize = new Vector3(128, 80);

    private WeaponData weaponData;

    public void Init(WeaponData data) 
    {
        weaponData = data;
        icon.sprite = weaponData.weaponIcon;
        SetSelected(false);
    }
    public void SetSelected(bool selected) 
    {
        Vector2 size = selected ? selectedSize : normalSize;

        rootRect.sizeDelta = size;

        if(background != null)
            background.rectTransform.sizeDelta = normalSize;
        if (icon != null)
            icon.rectTransform.sizeDelta = size;

        WeaponColumnUi column = GetComponent<WeaponColumnUi>();
        if (column != null)
            column.RefeshWidth();
    }

}
