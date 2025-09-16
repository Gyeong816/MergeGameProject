using UnityEngine;
using UnityEngine.UI;

public class DictionarySlot : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetData(bool isUnlocked, Sprite iconSprite)
    {
        if (iconSprite != null)
            icon.sprite = iconSprite;
        
        icon.color = isUnlocked ? Color.white : Color.black;
    }
}