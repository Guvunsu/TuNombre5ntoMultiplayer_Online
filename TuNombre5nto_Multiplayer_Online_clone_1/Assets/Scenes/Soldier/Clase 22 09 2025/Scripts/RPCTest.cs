using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class RPCTest : NetworkBehaviour
{
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessageTo_ServerRPC("Hi, I am client " + OwnerClientId);
        }
    }
    [ServerRpc]
    void SendMessageTo_ServerRPC(string msg, ServerRpcParams rpcparams = default)
    {
        Debug.Log("[Server] recived" + msg);
        ResendMessageToAll_ClientRPC("Server is sending " + msg);
    }

    [ClientRpc]
    void ResendMessageToAll_ClientRPC(string msg, ClientRpcParams rpcparams = default)
    {
        Debug.Log("[Client]" + msg);
    }
}
