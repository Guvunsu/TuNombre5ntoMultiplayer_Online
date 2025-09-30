using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;

public class UIShopItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameTag;
    public TMP_Text description;
    public TMP_Text priceTag;
    public Image rarityShadow;
    public Button buyButton;
    private UiShopManagerDownBecauseIWroteViceverseNames currentItem;
    private Sprite currentIcon;
    private ShopItem shopItemManager;
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
    public void OnBuyButtonPressed()
    {
        UserDataManagement userManager = FindObjectOfType<UserDataManagement>();

        if (userManager != null)
        {
            // ?? Aquí podrías hacer validación con item.price
            userManager.PurchaseCredits();
            userManager.SaveProfile();
            Debug.Log("Compra realizada con éxito");
        } else
        {
            Debug.LogWarning("No se pudo comprar: UserDataManagement no encontrado");
        }
    }
}
