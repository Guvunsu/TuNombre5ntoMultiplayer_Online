using UnityEngine;

public class UIFlowController : MonoBehaviour
{
    public GameObject panelLogin;
    public GameObject panelRol;
    public GameObject panelShop;
    public GameObject panelLoading;

    public UserAuth auth;
    public UserDataManagement data;

    private bool isHost = false;

    void Start()
    {
        ShowLogin();
    }

    public void ShowLogin()
    {
        panelLogin.SetActive(true);
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

    public void SelectHost()
    {
        isHost = true;
        ShowShop();
    }

    public void SelectClient()
    {
        isHost = false;
        ShowShop();
    }

    public void ShowShop()
    {
        panelLogin.SetActive(false);
        panelRol.SetActive(false);
        panelShop.SetActive(true);
        panelLoading.SetActive(false);

        data.SaveProfile();
    }

    public void PlayGame()
    {
        panelLogin.SetActive(false);
        panelRol.SetActive(false);
        panelShop.SetActive(false);
        panelLoading.SetActive(true);

        if (isHost)
            FindAnyObjectByType<MultiplayerBootStarp>().Host();
        else
            FindAnyObjectByType<MultiplayerBootStarp>().QuickJoin();
    }
}
