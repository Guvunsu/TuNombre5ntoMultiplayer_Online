using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameTag;
    public TMP_Text description;
    public TMP_Text priceTag;
    public Image rarityShadow;

    public void BindData(UiShopManagerDownBecauseIWroteViceverseNames item, Sprite sprite)
    {
        nameTag.text = item.name;
        description.text = item.description;
        priceTag.text = item.price;
        icon.sprite = sprite;

        if (item.rarity == "cammon")
            rarityShadow.color = Color.green;
        if (item.rarity == "Uncammon")
            rarityShadow.color = Color.yellow;
    }
}
