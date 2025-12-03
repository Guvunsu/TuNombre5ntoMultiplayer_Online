using Unity.Netcode;
using UnityEngine;

public class WallLife : NetworkBehaviour
{
    [Header("Wall Settings")]
    [SerializeField] public int maxHealth = 3;

    private NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    private void Start()
    {
        if (IsServer)
            currentHealth.Value = maxHealth;

        currentHealth.OnValueChanged += OnHealthChanged;
    }

    public void OnDestroy()
    {
        currentHealth.OnValueChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int previous, int current)
    {
        Debug.Log($"Muro ahora tiene {current} vidas");
    }

    [ServerRpc(RequireOwnership = false)]
    public void DamageWallServerRpc(int damage = 1)
    {
        if (!IsServer) return;

        currentHealth.Value -= damage;

        if (currentHealth.Value <= 0)
        {
            Debug.Log("El muro fue destruido");
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;

        if (collision.collider.CompareTag("Titan"))
        {
            DamageWallServerRpc(1);
        }
    }
}
