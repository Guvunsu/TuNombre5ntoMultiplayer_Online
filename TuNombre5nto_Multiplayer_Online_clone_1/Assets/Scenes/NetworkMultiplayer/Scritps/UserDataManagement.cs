using UnityEngine;
using System;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

[Serializable]
public class UserData
{
    public string userName;
    public float KD;
    public int credits;
}

public class UserDataManagement : MonoBehaviour
{
    public UserData userData;

    DatabaseReference databaseReference;
    FirebaseAuth auth;
    string userID;

    private async void Awake()
    {
        var depedencias = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (depedencias != DependencyStatus.Available)
        {
            Debug.LogError("Not Available: " + depedencias);
            return;
        }

        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            Debug.LogError("No user authenticated!");
            return;
        }

        userID = auth.CurrentUser.UserId;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.LogWarning("Firebase ready. Current UID: " + userID);
    }

    public void SaveProfile()
    {
        if (!IsReady()) return;

        string json = JsonUtility.ToJson(userData);
        databaseReference.Child("users").Child(userID).Child("profile")
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("Save canceled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Save failed: " + task.Exception);
                    return;
                }
                Debug.Log("Profile saved successfully for UID: " + userID);
            });
    }

    public void SavePurchase(UiShopManagerDownBecauseIWroteViceverseNames item)
    {
        if (!IsReady()) return;

        string purchaseId = databaseReference.Child("users").Child(userID).Child("purchases").Push().Key;
        string json = JsonUtility.ToJson(item);

        databaseReference.Child("users").Child(userID).Child("purchases").Child(purchaseId)
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("Purchase save canceled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Purchase save failed: " + task.Exception);
                    return;
                }
                Debug.Log("Purchase saved successfully: " + item.name);
            });
    }

    private bool IsReady()
    {
        if (auth == null || databaseReference == null)
        {
            Debug.LogError("Firebase not yet initialized");
            return false;
        }
        if (auth.CurrentUser == null)
        {
            Debug.LogError("No user detected");
            return false;
        }
        if (string.IsNullOrEmpty(userID))
        {
            userID = auth.CurrentUser.UserId;
        }
        return true;
    }

    public void PurchaseCredits()
    {
        if (!IsReady())
        {
            Debug.LogWarning("No se pudo comprar: datos incompletos");
            return;
        }

        // Sumar créditos
        userData.credits += 100;
        Debug.Log($"Créditos actuales: {userData.credits}");

        // Guardar en Firebase
        SaveProfile();
    }

}
