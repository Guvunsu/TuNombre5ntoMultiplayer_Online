using UnityEngine;
using Unity.Netcode;

public class NetworketProjectile : NetworkBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] float lifeSeconds = 10;

    Vector3 dir = Vector3.forward;
    float elapsedTime;

    public void Initialize(Vector3 direction)
    {
        dir = direction;
    }
    private void Update()
    {
        if (!IsServer) return;
        transform.position += dir * speed * Time.deltaTime;
        elapsedTime = Time.deltaTime;
        if (elapsedTime > lifeSeconds)
        {
            if (NetworkObject != null && NetworkObject.IsSpawned)
            {
                NetworkObject.Despawn();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) { return; }
        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn();
        }
    }
}
