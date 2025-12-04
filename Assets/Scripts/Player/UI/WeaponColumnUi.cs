using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WeaponColumnUi : MonoBehaviour
{
    private RectTransform _rectTransform;
    private LayoutElement _layoutElement;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _layoutElement = GetComponent<LayoutElement>();
    }
    public void RefeshWidth() 
    {
        float maxWidth = 0f;
        foreach(Transform child in transform) 
        {
            var le = child.GetComponent<LayoutElement>();
            if(le != null && le.preferredHeight > maxWidth)
                maxWidth = le.preferredWidth;
        }
        if (_layoutElement != null)
        {
            _layoutElement.preferredWidth = maxWidth;
        }
        else 
        {
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
        }

        var parentRect = _rectTransform.parent as RectTransform;
        if(parentRect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
    }
}
