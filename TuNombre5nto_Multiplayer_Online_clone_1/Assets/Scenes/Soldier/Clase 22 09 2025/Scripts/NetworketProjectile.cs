using Unity.Netcode;
using UnityEngine;

public class NetworketProjectile : NetworkBehaviour
{
    public float speed = 15f;
    Vector3 direction;

    public void Initialize(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Titan"))
        {
            Health h = other.GetComponent<Health>();
            if (h != null)
            {
                h.TakeDamageServerRpc(100f);
            }
        }

        GetComponent<NetworkObject>().Despawn();
    }
}
