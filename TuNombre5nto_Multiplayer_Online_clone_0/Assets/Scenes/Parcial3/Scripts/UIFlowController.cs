using UnityEngine;
using Unity.Netcode;
using Firebase.Auth;

public class UIFlowController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject panelLogin;
    public GameObject panelRol;
    public GameObject panelShop;
    public GameObject panelLoading;

    public UserAuth auth;
    public UserDataManagement data;

    private bool isHost = false;
    private bool authenticated = false;
    private bool requestedMultiplayer = false;

    private void Awake()
    {
        // Firebase
        FirebaseAuth fbAuth = FirebaseAuth.DefaultInstance;

        if (fbAuth.CurrentUser != null)
        {
            authenticated = true;
            ShowMenu();
        } else
        {
            ShowLogin();
        }
    }

    private void OnEnable()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    // ============================================================
    //                UI FLOW ORIGINAL + Firebase + Netcode
    // ============================================================

    void Start()
    {
         
        ShowLogin();
        if (!authenticated)
        {
            ShowLoading();
        }
    }

    public void ShowLogin()
    {
        panelLogin.SetActive(true);
        panelRol.SetActive(false);
        panelShop.SetActive(false);
        panelLoading.SetActive(false);
    }

    void ShowMenu()
    {
        panelLogin.SetActive(false);
        panelRol.SetActive(false);
        panelShop.SetActive(false);
        panelLoading.SetActive(false);
    }

    public void ShowShop()
    {
        panelLogin.SetActive(false);
        panelRol.SetActive(false);
        panelShop.SetActive(true);
        panelLoading.SetActive(false);

        data.SaveProfile();
    }

    void ShowLoading()
    {
        panelLogin.SetActive(false);
        panelRol.SetActive(false);
        panelShop.SetActive(false);
        panelLoading.SetActive(true);
    }

    public void ShowGame()
    {
        panelLogin.SetActive(false);
        panelRol.SetActive(false);
        panelShop.SetActive(false);
        panelLoading.SetActive(false);
    }
    public void ShowRol()
    {
        panelLogin.SetActive(false);
        panelRol.SetActive(true);
        panelShop.SetActive(false);
        panelLoading.SetActive(false);
    }
    // ============================================================
    //                     AUTH + BOTONES
    // ============================================================

    public void OnPlayerAuthenticated()
    {
        authenticated = true;
        ShowMenu();
    }

    public void SelectHost()
    {
        isHost = true;
        PlayGame();
    }

    public void SelectClient()
    {
        isHost = false;
        PlayGame();
    }

    public void PlayGame()
    {
        requestedMultiplayer = true;

        ShowLoading();

        MultiplayerBootStarp boot = FindAnyObjectByType<MultiplayerBootStarp>();

        if (isHost)
            boot.Host();
        else
            boot.QuickJoin();
    }

    // ============================================================
    //                 NETCODE CALLBACK – AL CONECTAR
    // ============================================================

    private void OnClientConnected(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
            return;

        if (!requestedMultiplayer)
            return;

        ShowGame();
    }
}
