using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using UnityEngine;

using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

using Unity.Services.Core;
using Unity.Services.Authentication;

using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class MultiplayerBootStarp : MonoBehaviour
{

    [Header("Lobby")]
    public string lobbyName = "ClassLobby";
    [Range(2, 16)] public int maxPlayers = 4; // total incl. host

    void Awake() => _ = EnsureUgsReady();

    private async System.Threading.Tasks.Task EnsureUgsReady()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
            await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Host flow: create Relay allocation -> get join code -> create Lobby -> start Host
    public async void Host()
    {
        try
        {
            var alloc = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
            Debug.Log($"[Host] Relay Join Code: {joinCode}");

            var lobbyData = new Dictionary<string, DataObject> {
                { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(
                lobbyName, maxPlayers,
                new CreateLobbyOptions { IsPrivate = false, Data = lobbyData });

            var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
            // ✅ Use AllocationUtils to build the correct RelayServerData for your package versions
            utp.SetRelayServerData(AllocationUtils.ToRelayServerData(alloc, "dtls"));

            NetworkManager.Singleton.StartHost();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Host] {e}");
        }
    }

    // Client flow: quick-join any lobby -> read join code -> join Relay -> start Client
    public async void QuickJoin()
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            if (!joinedLobby.Data.TryGetValue("joinCode", out var dataObj))
            {
                Debug.LogError("[QuickJoin] Lobby missing joinCode.");
                return;
            }
            string code = dataObj.Value;

            var joinAlloc = await RelayService.Instance.JoinAllocationAsync(code);

            var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
            // ✅ Same here
            utp.SetRelayServerData(AllocationUtils.ToRelayServerData(joinAlloc, "dtls"));

            NetworkManager.Singleton.StartClient();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[QuickJoin] {e}");
        }
    }

    // Optional: join by manually typing a Relay join code
    public async void JoinWithCode(string code)
    {
        try
        {
            var joinAlloc = await RelayService.Instance.JoinAllocationAsync(code);

            var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
            utp.SetRelayServerData(AllocationUtils.ToRelayServerData(joinAlloc, "dtls"));

            NetworkManager.Singleton.StartClient();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[JoinWithCode] {e}");
        }
    }
}