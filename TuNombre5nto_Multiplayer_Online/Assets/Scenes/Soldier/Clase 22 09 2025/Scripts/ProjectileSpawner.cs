using Unity.Netcode;
using UnityEngine;

public class ProjectileSpawner : NetworkBehaviour
{
    [SerializeField] NetworkObject projectilePrefab;
    [SerializeField] Transform spawnPoint;

    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = spawnPoint ? spawnPoint.position : transform.position + transform.forward * 0.6f;
            Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;
            Vector3 dir = rot * Vector3.forward;

            SpawnProjectileServerRpc(pos, rot, dir);
        }
    }
    [ServerRpc]
    void SpawnProjectileServerRpc(Vector3 pos, Quaternion rot, Vector3 dir, ServerRpcParams _ = default)
    {
        var proj = Instantiate(projectilePrefab, pos, rot);
        var simple = proj.GetComponent<NetworketProjectile>();
        if (simple != null) simple.Initialize(dir);
        proj.Spawn();
    }
}
