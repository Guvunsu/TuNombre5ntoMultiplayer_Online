//using UnityEngine;
//using Unity.Netcode;
//using System.Collections;
////parcial 2
//public class MiniTitan : NetworkBehaviour
//{
//    [SerializeField] float speed = 2f;
//    [SerializeField] float lifetime = 15f;
//    [SerializeField] NetworkObject explosionPrefab;
//    public Vector3 dir = Vector3.forward;

//    public void Initialize(Vector3 direction)
//    {
//        dir = direction;
//    }
//    void Start()
//    {
//        if (IsServer)
//        {
//            StartCoroutine(SelfDestruct());
//        }
//    }

//    void Update()
//    {
//        //if (!IsServer) return;

//        transform.Translate(Vector3.forward * speed * Time.deltaTime);
//    }

//    IEnumerator SelfDestruct()
//    {
//        yield return new WaitForSeconds(lifetime);
//        ExplodeClientRpc();
//        yield return new WaitForSeconds(0.5f);
//        GetComponent<NetworkObject>().Despawn(true);
//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        if (!IsServer) return; // Solo el servidor controla la explosión y daño

//        if (other.CompareTag("Player"))
//        {
//            // 🧨 Explosión para todos
//            ExplodeClientRpc();

//            // 💥 Si quieres hacer daño al jugador (opcional según tu rubrica)
//            if (other.TryGetComponent<Health>(out var health))
//            {
//                health.TakeDamageServerRpc(10); // lo que tú quieras
//            }

//            StartCoroutine(DestroyAfterExplosion());
//        }
//    }

//    IEnumerator DestroyAfterExplosion()
//    {
//        yield return new WaitForSeconds(0.3f);
//        GetComponent<NetworkObject>().Despawn(true);
//    }

//    [ClientRpc]
//    void ExplodeClientRpc()
//    {
//        if (explosionPrefab != null)
//        {
//            NetworkObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
//            Destroy(fx, 2f);
//        }
//    }
//}

////parcial 3
using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class MiniTitan : NetworkBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float lifetime = 15f;
    [SerializeField] NetworkObject explosionPrefab;

    public Vector3 dir = Vector3.forward;

    public void Initialize(Vector3 direction)
    {
        dir = direction;
    }

    void Start()
    {
        if (IsServer)
            StartCoroutine(SelfDestruct());
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifetime);
        ExplodeClientRpc();
        yield return new WaitForSeconds(0.5f);
        GetComponent<NetworkObject>().Despawn(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        // 💥 Si golpea un muro
        if (other.CompareTag("Wall"))
        {
            // dañar el muro si tiene WallLife
            if (other.TryGetComponent<WallLife>(out var wall))
                wall.DamageWallServerRpc(1);

            ExplodeClientRpc();
            StartCoroutine(DestroyAfter());
        }

        // 💥 Si golpea un jugador
        if (other.CompareTag("Player"))
        {
            ExplodeClientRpc();

            if (other.TryGetComponent<Health>(out var health))
                health.TakeDamageServerRpc(10);

            StartCoroutine(DestroyAfter());
        }
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<NetworkObject>().Despawn(true);
    }

    [ClientRpc]
    void ExplodeClientRpc()
    {
        if (explosionPrefab != null)
        {
            var fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx.gameObject, 2f);
        }
    }
}
