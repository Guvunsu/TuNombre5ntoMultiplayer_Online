using System.Collections;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase;
using TMPro;
public class UserProfile : MonoBehaviour
{
    [Header("UI Info")]
    public TMP_Text userInfoText; // arrastra aquí tu TMP_Text

    private FirebaseAuth auth;

    private void OnEnable()
    {
        auth = FirebaseAuth.DefaultInstance;
        if (auth != null)
        {
            auth.StateChanged += OnAuthStateChanged;
            ShowCurrentUserInfo(); // muestra al iniciar si ya hay usuario
        }
    }

    private void OnDisable()
    {
        if (auth != null)
            auth.StateChanged -= OnAuthStateChanged;
    }

    private void OnAuthStateChanged(object sender, System.EventArgs e)
    {
        ShowCurrentUserInfo();
    }

    public void ShowCurrentUserInfo()
    {
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string info = $"UID: {user.UserId}\nUsuario: {user.DisplayName}\nCorreo: {user.Email}";
            Debug.Log("[UserProfile] " + info);
            if (userInfoText != null) userInfoText.text = info;
        } else
        {
            Debug.Log("[UserProfile] No hay usuario logueado.");
            if (userInfoText != null) userInfoText.text = "No hay usuario logueado";
        }
    }
}
