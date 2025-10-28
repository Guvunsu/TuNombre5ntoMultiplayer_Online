using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class MiniTitan : NetworkBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float lifetime = 15f;
    [SerializeField] GameObject explosionPrefab; 

    void Start()
    {
        if (IsServer)
        {
            StartCoroutine(SelfDestruct());
        }
    }

    void Update()
    {
        if (!IsServer) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifetime);
        ExplodeClientRpc();
        yield return new WaitForSeconds(0.5f);
        GetComponent<NetworkObject>().Despawn(true);
    }

    [ClientRpc]
    void ExplodeClientRpc()
    {
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }
    }
}

