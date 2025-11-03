using Unity.Netcode;
using UnityEngine;

public class ProjectileSpawner : NetworkBehaviour
{
    [SerializeField] NetworkObject projectilePrefab;
    [SerializeField] Transform spawnPoint;

    private void Update()
    {
        //if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Disparo");
            Vector3 pos = spawnPoint ? spawnPoint.position : transform.position + transform.forward * 0.6f;
            Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;
            Vector3 dir = rot * Vector3.forward;

            SpawnProjectileClientRpc(pos, rot, dir);
        }
    }
    [ClientRpc]
    void SpawnProjectileClientRpc(Vector3 pos, Quaternion rot, Vector3 dir, ClientRpcParams _ = default)
    {
        var proj = Instantiate(projectilePrefab, pos, rot);
        var simple = proj.GetComponent<NetworketProjectile>();
        if (simple != null) simple.Initialize(dir);
        proj.Spawn();
    }
}
