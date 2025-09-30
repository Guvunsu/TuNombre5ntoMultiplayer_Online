using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
public class UiShopManagerDownBecauseIWroteViceverseNames
{
    public string description;
    public string imagePath;
    public string name;
    public string price;
    public string rarity;
}
public class ShopItem : MonoBehaviour
{
    public Transform container;
    public GameObject skinCard;

    DatabaseReference databaseReference;
    bool firebaseIsReady = false;
    string path = "catalog/skins";
    async void Awake()
    {
        var depebdebcy = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (depebdebcy == DependencyStatus.Available)
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            firebaseIsReady = true;
        } else Debug.LogError("Firebase is unvailable");
    }
    public void LoadStore()
    {
        if (!firebaseIsReady) return;
        StartCoroutine(FillStore());
    }
    IEnumerator FillStore()
    {
        var task = databaseReference.Child(path).OrderByChild("active ").EqualTo(true).GetValueAsync();
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

            uiShopItem.BindData(shopItem, icon);
        }
    }
    Sprite LoadLocalSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        return Resources.Load<Sprite>(path);
    }
}