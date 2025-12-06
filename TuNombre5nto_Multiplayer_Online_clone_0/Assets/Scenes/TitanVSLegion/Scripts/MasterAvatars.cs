using Unity.Netcode;
using UnityEngine;


public class MasterAvatars : NetworkBehaviour
{
    private NetworkObject networkObject;
    // quitar el comentado para que sirva parcial 2, haci solo comentado sirve para el parcial 3
    public GameObject HostTitan, ClientTitan;
    public GameObject HostLegion, ClientLegion;

    // NUEVO: punto de respawn para la Legión 
    [SerializeField] Transform legionRespawnPoint;

    // NUEVO: contador de muertes sincronizado
    private NetworkVariable<int> legionDeaths = new NetworkVariable<int>(0);

    // NUEVO: panel game over (solo se activa en la máquina dueña de la Legión)
    [SerializeField] GameObject gameOverPanel;

    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            bool isOwner = networkObject.IsOwner;
        //TITAN: soy el host si mi ID es 0
            if (networkObject.OwnerClientId == 0)
            {
                if (isOwner) HostTitan.SetActive(true);
                //escribir el transform de donde aparecera el titan
                else ClientTitan.SetActive(true); //parcial 2
                //escribir el transform de donde aparecera la legion
            }
            //parcial 2 hay que descomentarlo para que funcione
            //Legion soy el cliente y el Id es > 0
            else
            {
                if (isOwner) ClientLegion.SetActive(true);
                else HostLegion.SetActive(true);

            }
        }
    }
    // ESTE MÉTODO LO LLAMAS CUANDO LA LEGIÓN MUERE (Desde su script de vida)
    [ServerRpc(RequireOwnership = false)]
    public void LegionDiedServerRpc()
    {
        legionDeaths.Value++;

        if (legionDeaths.Value < 3)
        {
            // Respawn 1 y 2
            RespawnLegionClientRpc();
        } else
        {
            // Muerte 3: GameOver
            GameOverClientRpc();
        }
    }

    // Respawn visual SOLO para el dueño de la Legión
    [ClientRpc]
    private void RespawnLegionClientRpc()
    {
        if (!IsOwner) return;

        Debug.Log("Respawn Legión");

        // Reset posición
        Transform avatar = ClientLegion.activeSelf ? ClientLegion.transform : HostLegion.transform;
        avatar.position = legionRespawnPoint.position;
    }

    //  Activar panel final SOLO del dueño de la Legión
    [ClientRpc]
    private void GameOverClientRpc()
    {
        if (!IsOwner) return;

        Debug.Log("GAME OVER: Legión murió 3 veces");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void OnLegionDeathCountChanged(int oldVal, int newVal)
    {
        Debug.Log($"Legion muertes: {newVal}");
    }
}
