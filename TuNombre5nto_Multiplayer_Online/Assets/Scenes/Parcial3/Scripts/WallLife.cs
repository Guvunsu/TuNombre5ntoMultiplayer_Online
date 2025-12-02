using Unity.Netcode;
using UnityEngine;

public class WallLife : NetworkBehaviour
{
    [Header("Wall Settings")]
    [SerializeField] private int maxHealth = 3;

    // Vida sincronizada entre todos los jugadores
    private NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    private void Start()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }

        currentHealth.OnValueChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        currentHealth.OnValueChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int previous, int current)
    {
        Debug.Log($"Muro ahora tiene {current} vidas");
    }

    /// <summary>
    /// El proyectil debe llamar a esto cuando colisiona con el muro.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void DamageWallServerRpc(int damage = 1)
    {
        if (!IsServer) return;

        currentHealth.Value -= damage;

        if (currentHealth.Value <= 0)
        {
            Debug.Log("⚠️ El muro fue destruido");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Los minititanes o proyectiles deben tener este tag
        if (!IsServer) return;

        if (other.CompareTag("TitanProjectile"))
        {
            DamageWallServerRpc(1);
        }
    }
}
