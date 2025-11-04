using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ProjectileSpawner : NetworkBehaviour
{
    Health script_Health;
    [SerializeField] NetworkObject projectilePrefab;
    [SerializeField] Transform spawnPoint;
    private void Start()
    {
        Debug.Log("Si existo por favor ayudame");
    }
    private void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Disparo");
            Vector3 pos = spawnPoint ? spawnPoint.position : transform.position + transform.forward * 0.6f;
            Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;
            Vector3 dir = rot * Vector3.forward;

            SpawnProjectileServerRpc(pos, rot, dir);
        }
        if (IsServer)
        {

        }


    }
    [ServerRpc]
    void SpawnProjectileServerRpc(Vector3 pos, Quaternion rot, Vector3 dir, ServerRpcParams _ = default)
    {
        Debug.LogWarning("Detengo el ServerRPC");
        //var proj = Instantiate(projectilePrefab, pos, rot);
        //var simple = proj.GetComponent<NetworketProjectile>();
        //if (simple != null) simple.Initialize(dir);
        //proj.Spawn();
        SpawnProjectileClientRpc(pos, rot, dir);
    }
    [ClientRpc]
    void SpawnProjectileClientRpc(Vector3 pos, Quaternion rot, Vector3 dir, ClientRpcParams _ = default)
    {
        Debug.Log("Detengo el ClientRPC");
        var proj = Instantiate(projectilePrefab, pos, rot);
        var simple = proj.GetComponent<NetworketProjectile>();
        if (simple != null) simple.Initialize(dir);
        proj.Spawn();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Titan"))
        {
            script_Health.TakeDamage(10);

        }
    }
}
