using UnityEngine;
using Unity.Netcode;
// en Unity, lo RPC solo pueden usarse si el script hereda de NetworkBehaviour (Unity.Netcode)
public class RPCBehaviourExample : NetworkBehaviour
{
    // Los RPC (REMOTE PROCEDURE CALL) SON FUNCIONES QUE SE PUEDEN
    // Detonar desde el client o servidor, para "obligar" a otras instancias del juego
    // a ejecutar acciones especeficas
    //Si se quiere mandar un RPC desde el server: SERVER -> client(s), es mas directo
    // Si se quiere mandar un RPC desde el cliente: Client -> Server -> client(s). Primero por SERVER

    //Para poder enviar RPC´S necesitamos usar el tag [ServerRPC] o [ClientRPC]
    //Las funciones deben llevar el sufijo ClientRPC o ServerRPC segun sea el caso

    //Ejemplo 1: Llamada directa de server a todos los demas, con argumento
    public PlayerDataNetExample script_PlayerDataNetExample;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SayHello_ServerRPC("Hola soy el ejemplo");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            script_PlayerDataNetExample.goldCount = 49;
            script_PlayerDataNetExample.killPosition = transform.position;
            SpawnLootWhenKilled_ServerRPC(script_PlayerDataNetExample);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //script_PlayerDataNetExample.goldCount
        }
    }
    [ServerRpc]
    void SayHello_ServerRPC(string message)
    {
        Debug.Log("Hola soy el ejemplo");
        ApplySayHello_ClientRPC();
    }
    [ClientRpc]
    void ApplySayHello_ClientRPC()
    {
        Debug.Log("Hola,soy el cliente y estoy recibiendo un mensaje ");
    }

    // Ejemplo 2: ServerRPC con informacion serializada

    [ServerRpc]
    void SpawnLootWhenKilled_ServerRPC(PlayerDataNetExample data)
    {
        SpawnLootWhenKilled_ClientRPC(data);
    }
    [ClientRpc]
    void SpawnLootWhenKilled_ClientRPC(PlayerDataNetExample data)
    {
        Debug.Log("spawn:" + data.goldCount + "at:" + data.killPosition);
    }

    // ejemplo 3: Cliente a server, para avisar a todos los demas
    [ClientRpc]
    void BroadcastDeadEvent_ClientRPC(PlayerDataNetExample data)
    {
        BroadcastToEveryone_ServerRPC(data);
    }

    [ServerRpc]
    void BroadcastToEveryone_ServerRPC(PlayerDataNetExample data)
    {
        BroadcastDeadEvent_ClientRPC(data);
    }

    [ClientRpc]
    void ReceiveBrodcast_ClientRPC(PlayerDataNetExample data)
    {
        Debug.Log("spawn" + data.goldCount + "at:" + data.killPosition);
    }
}
