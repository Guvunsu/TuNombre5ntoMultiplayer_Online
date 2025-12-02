using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    [Header("Configuración de vida")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] Image healthFill; // Imagen tipo Fill (barra de vida)

    private NetworkVariable<float> currentHealth = new NetworkVariable<float>(
        100f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    void Start()
    {
        if (IsServer)
            currentHealth.Value = maxHealth;

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

    void UpdateHealthBar(float value)
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = value / maxHealth;
        }
    }
   // [ServerRPC]//HACER QUE EL EVENTO DESDE EL HOST CUANDO EL TITAN RECIBA EL IMPACTO Y DESDE CONSOLA LLAMAR QUE LE HAN PEGADO DESDE EL TITAN
   // DESDE UN COLLIONENTER LLAMAR EL RPC PARA RECIBIR DAÑO
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float amount)
    {
        currentHealth.Value = Mathf.Max(0, currentHealth.Value - amount);

        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} murió (despawn)");
        GetComponent<NetworkObject>().Despawn(); // Se elimina para todos los jugadores
    }

    [ServerRpc(RequireOwnership = false)]
    public void RestoreFullHealthServerRpc()
    {
        currentHealth.Value = maxHealth;
    }
}
