using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using Firebase.Auth;

[System.Serializable]
public class UiShopManagerDownBecauseIWroteViceverseNames
{
    public string description;
    public string imagePath;
    public string name;
    public string price;
    public string rarity;
}
//parcial 3
public static class EquippedItems
{
    public static string skinID;
    public static string projectileID;
}// aca termina
public class ShopItem : MonoBehaviour
{
    public Transform container;
    public GameObject skinCard;
    DatabaseReference databaseReference;
    bool firebaseIsReady = false;
    string path = "catalog/skins";
    string playerId;
    void Start()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            playerId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            Debug.Log("UID del jugador: " + playerId);
        }
        else
        {
            Debug.LogError("No hay usuario autenticado.");
        }
    }
    async void Awake()
    {
        var depebdebcy = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (depebdebcy == DependencyStatus.Available)
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            firebaseIsReady = true;
        }
        else Debug.LogError("Firebase is unvailable");
    }
    //parcial 3
    public void EquipSkin(string id)
    {
        EquippedItems.skinID = id;
    }

    public void EquipProjectile(string id)
    {
        EquippedItems.projectileID = id;
    }
    // aca termina parcial 3
    public void LoadStore()
    {
        if (!firebaseIsReady) return;
        StartCoroutine(FillStore());
    }
    IEnumerator FillStore()
    {
        var task = databaseReference.Child(path).OrderByChild("active").EqualTo(true).GetValueAsync();
        while (!task.IsCompleted) yield return null;
        if (task.IsFaulted)
        {
            Debug.LogError("Error loading store" + task.Exception.ToString());
            yield break;
        }
        var snap = task.Result;
        if (!snap.Exists) yield break;
        foreach (var child in snap.Children)
        {
            var shopItem = JsonUtility.FromJson<UiShopManagerDownBecauseIWroteViceverseNames>(child.GetRawJsonValue());

            var skinInstance = Instantiate(skinCard, container);
            UIShopItem uiShopItem = skinInstance.GetComponent<UIShopItem>();

            var icon = LoadLocalSprite(shopItem.imagePath);

            Debug.Log($"muestro imagen" + shopItem.imagePath);

            uiShopItem.BindData(shopItem, icon);
        }
    }
    Sprite LoadLocalSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        return Resources.Load<Sprite>(path);
    }

    public void SavePurchase(UiShopManagerDownBecauseIWroteViceverseNames item)
    {
        if (!firebaseIsReady)
        {
            Debug.LogError("Firebase no está listo, no se puede guardar.");
            return;
        }

        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("PlayerId vacío, no se puede guardar.");
            return;
        }

        string purchaseId = databaseReference.Child("users").Child(playerId).Child("purchases").Push().Key;

        string json = JsonUtility.ToJson(item);
        Debug.Log("Guardando JSON: " + json);

        databaseReference.Child("users").Child(playerId).Child("purchases").Child(purchaseId)
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("Compra guardada correctamente: " + item.name);
                else
                    Debug.LogError("Error al guardar compra: " + task.Exception);
            });
    }

}