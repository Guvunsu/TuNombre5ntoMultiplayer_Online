using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    [Header("Configuración de vida")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] Scrollbar healthBar; 

    // Variable sincronizada entre clientes
    private NetworkVariable<float> currentHealth = new NetworkVariable<float>(100f,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);

    void Start()
    {
        if (IsServer)
            currentHealth.Value = maxHealth;

        // Escucha los cambios para actualizar la UI en todos los clientes
        currentHealth.OnValueChanged += OnHealthChanged;
        UpdateHealthBar(currentHealth.Value);
    }

    void OnDestroy()
    {
        currentHealth.OnValueChanged -= OnHealthChanged;
    }

    void OnHealthChanged(float oldValue, float newValue)
    {
        UpdateHealthBar(newValue);
    }

    void UpdateHealthBar(float newValue)
    {
        if (healthBar != null)
        {
            healthBar.size = newValue / maxHealth;
        }
    }

    /// <summary>
    /// Llamar desde otro script el script del proyectil para hacer daño
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (!IsServer) return;

        currentHealth.Value = Mathf.Max(0, currentHealth.Value - amount);
    }

    /// <summary>
    /// Método auxiliar opcional para restaurar la vida completa
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void RestoreFullHealthServerRpc()
    {
        currentHealth.Value = maxHealth;
    }
}
