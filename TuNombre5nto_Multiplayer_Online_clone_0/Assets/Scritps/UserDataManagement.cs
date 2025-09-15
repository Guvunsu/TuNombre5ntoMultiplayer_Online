using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Unity.VisualScripting;

[Serializable]
public class UserData
{
    public string userName;
    public float KD;
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
            Debug.LogError("Not Available" + depedencias);
            return;
        }
      auth = FirebaseAuth.DefaultInstance;

        userID = auth.CurrentUser.UserId;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.LogWarning("Firebase is ready . current uid" + userID);
    }
    public void SaveData()
    {
        if (!IsReady()) return;

        string json = JsonUtility.ToJson(userData);
        databaseReference.Child("users").Child(userID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("save canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Save failed" + task.Exception); return;
            }
            Debug.Log("Save succesful for UID" + userID);

        });

    }
    private bool IsReady()
    {
        if (auth == null || databaseReference == null)
        {
            Debug.LogError("firebase is not yet initialized");
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
}
