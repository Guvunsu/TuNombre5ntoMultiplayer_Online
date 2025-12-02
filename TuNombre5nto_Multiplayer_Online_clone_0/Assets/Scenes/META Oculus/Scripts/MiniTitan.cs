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
        {
            StartCoroutine(SelfDestruct());
        }
    }

    void Update()
    {
        //if (!IsServer) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
        if (!IsServer) return; // Solo el servidor controla la explosión y daño

        if (other.CompareTag("Player"))
        {
            // 🧨 Explosión para todos
            ExplodeClientRpc();

            // 💥 Si quieres hacer daño al jugador (opcional según tu rubrica)
            if (other.TryGetComponent<Health>(out var health))
            {
                health.TakeDamageServerRpc(10); // lo que tú quieras
            }

            StartCoroutine(DestroyAfterExplosion());
        }
    }

    IEnumerator DestroyAfterExplosion()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<NetworkObject>().Despawn(true);
    }

    [ClientRpc]
    void ExplodeClientRpc()
    {
        if (explosionPrefab != null)
        {
            NetworkObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }
    }
}

