using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class SpawnTitan : NetworkBehaviour
{
    [SerializeField] NetworkObject miniTitanPrefab;
    [SerializeField] Transform spawnPoint;
    //[SerializeField] float spawnOffset = 2f;


    private void Start()
    {
        miniTitanPrefab.GetComponent<NetworkObject>();
        if (miniTitanPrefab != null)
        {
            bool IsOwner = true;
            IsOwner = miniTitanPrefab.IsOwner;
        }
    }
    void Update()
    {
        //if (!IsOwner) return; // <- usa el IsOwner REAL de NetworkBehaviour

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Acciono el teclado spacebar");
            for (int i = -1; i < 2; i++)
            {
                Vector3 pos = spawnPoint ? spawnPoint.position : transform.position + transform.forward * 0.6f;
                Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;
                Vector3 dir = rot * Vector3.forward;
                SpawnMiniTitansServerRpc(pos, rot, dir);
            }
        }
    }
    [ServerRpc]
    void SpawnMiniTitansServerRpc(Vector3 pos, Quaternion rot, Vector3 dir, ServerRpcParams rpcParams = default)
    {
        var proj = Instantiate(miniTitanPrefab, pos, rot);
        var simple = proj.GetComponent<MiniTitan>();
        if (simple != null) simple.Initialize(dir);
        proj.Spawn();
    }
}
