using UnityEngine;
using Unity.Netcode;
public class ProjectileSpawnerTitan : NetworkBehaviour
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
        //if (IsServer)
        //{

        //}
    }
    [ServerRpc]
    void SpawnProjectileServerRpc(Vector3 pos, Quaternion rot, Vector3 dir, ServerRpcParams _ = default)
    {
        //Debug.LogWarning("Detengo el ServerRPC");
        var proj = Instantiate(projectilePrefab, pos, rot);
        var simple = proj.GetComponent<NetworketProjectile>();
        if (simple != null) simple.Initialize(dir);
        proj.Spawn();
        //SpawnProjectileClientRpc(pos, rot, dir);
    }
    //[ClientRpc]
    //void SpawnProjectileClientRpc(Vector3 pos, Quaternion rot, Vector3 dir, ClientRpcParams _ = default)
    //{
    //    Debug.Log("Detengo el ClientRPC");
    //    var proj = Instantiate(projectilePrefab, pos, rot);
    //    var simple = proj.GetComponent<NetworketProjectile>();
    //    if (simple != null) simple.Initialize(dir);
    //    proj.Spawn();
    //}
    // scene parcial 3 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall")) // le agrege el legion a ver sdi funciona con 4eso 
        {
            if (other.TryGetComponent<WallLife>(out var wall))
            {
                wall.DamageWallServerRpc(1);
            }

            // destruir mini titán o proyectil
            if (IsServer)
                GetComponent<NetworkObject>().Despawn();
        }
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<HealthBarRPC>(out var life))
            {
                life.TakeDamageServerRpc(10f);
            }

            if (IsServer)
                GetComponent<NetworkObject>().Despawn();

            return;
        }
    }
}
