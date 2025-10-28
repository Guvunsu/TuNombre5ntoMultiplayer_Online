using UnityEngine;
using Unity.Netcode;

public class TitanSpawner : NetworkBehaviour
{
    [SerializeField] GameObject miniTitanPrefab; 
    [SerializeField] Transform spawnPoint;      
    [SerializeField] float spawnOffset = 2f;  

    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnMiniTitansServerRpc();
        }
    }

    [ServerRpc]
    void SpawnMiniTitansServerRpc(ServerRpcParams rpcParams = default)
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = spawnPoint.forward * (i * spawnOffset);
            GameObject mini = Instantiate(miniTitanPrefab, spawnPoint.position + offset, spawnPoint.rotation);
            mini.GetComponent<NetworkObject>().Spawn(true);
        }
    }
}

