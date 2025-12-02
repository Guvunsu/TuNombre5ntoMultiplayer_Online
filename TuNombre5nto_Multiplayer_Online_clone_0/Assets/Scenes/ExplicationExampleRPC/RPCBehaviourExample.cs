using UnityEngine;
using Unity.Netcode;

// En Unity, lo RPC solo pueden usarse si el script hereda de NetworkBehaviour (Unity.Netcode).
public class RPCBehaviourExample : NetworkBehaviour
{

    public PlayerDataNet playerData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SayHello_ServerRPC();

        if (Input.GetKeyUp(KeyCode.B))
        {
            playerData.goldCount = 49;
            playerData.killPosition = transform.position;
            SpawnLootWhenKilled_ServerRPC(playerData);
        }

        if(Input.GetKeyUp(KeyCode.C))
        {
            playerData.goldCount = 49;
            playerData.killPosition = transform.position;
            SendMessageClientToOthers(playerData);
        }

    }

    // Los RPC (Remote Procedure Call) son funciones que se pueden
    // detonar desde el client o servidor, para "obligar" a otras instancias del juego
    // a ejecutar accionbes específicas.
    // Si se quiere mandar un rpc desde server: SERVER -> CLIENT(S). Es más directo.
    // Si se quiere mandar un rpc desde cliente: CLIENT -> SERVER -> CLIENT(S). Primero por SERVER.

    // Para poder enviar RPC's necesitamos usar el TAG [ServerRpc] o [ClientRpc]
    // Las funciones deben llevar el sufijo ClientRPC o ServerRPC según sea el caso.

    // Ejemplo 1: Llamada directa de server a todos los demás, con argumento.

    [ServerRpc]

    void SayHello_ServerRPC()
    {
        Debug.Log("Hola soy el ejemplo 1");
        ApplySayHello_ClientRPC();
    }

    [ClientRpc]
    void ApplySayHello_ClientRPC()
    {
        Debug.Log("Hola, soy el cliente y estoy recibiendo el rpc del server");
    }


    // Ejemplo 2: ServerRPC con infromación serializada.

    [ServerRpc]
    void SpawnLootWhenKilled_ServerRPC(PlayerDataNet data)
    {
        ApplySpawnLootWhenKilled_ClientRPC(data);
    }

    [ClientRpc]
    void ApplySpawnLootWhenKilled_ClientRPC(PlayerDataNet data)
    {
        Debug.Log("Spawn: " + data.goldCount + " at: " + data.killPosition);
    }

    // Ejemplo 3: Cliente a server, para avisar a todos los demás.


    void SendMessageClientToOthers(PlayerDataNet data)
    {
        if(IsClient && !IsServer)
        {
            BroadcastToEveryone_ServerRPC(data);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void BroadcastToEveryone_ServerRPC(PlayerDataNet data)
    {
        ReceiveBroadcast_ClientRPC(data);
    }

    [ClientRpc]
    void ReceiveBroadcast_ClientRPC(PlayerDataNet data)
    {
        Debug.Log("Spawn: " + data.goldCount + " at: " + data.killPosition);
    }

}
