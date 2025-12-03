using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarRPCTitan : NetworkBehaviour
{
    [Header("Barra De Vida")]
    public Image imageHealthBar;

    // Vida sincronizada para que todos vean lo mismo
    private NetworkVariable<float> currentLife = new NetworkVariable<float>(100f);

    [Header("Referencias")]
    public UIFlowController script_UIFlowController;

    // Para identificar si este avatar es Titan o Legion
    [SerializeField] bool isLegion = false;
     
    void Start()
    {
        currentLife.OnValueChanged += OnLifeChanged;
    }

    private void OnDestroy()
    {
        currentLife.OnValueChanged -= OnLifeChanged;
    }

    private void OnLifeChanged(float oldValue, float newValue)
    {
        // Actualizar barra en el cliente dueño del personaje
        if (IsOwner)
        {
            imageHealthBar.fillAmount = newValue / 100f;
        }
    }

    // Detecta colisiones con bala o minititan
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("MiniTitan"))
        {
            Debug.Log($"Me hizo daño el desgraciado: {other.gameObject.name}");

            // Pido daño al servidor
            TakeDamageServerRpc(10f);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void TakeDamageServerRpc(float damage)
    {
        currentLife.Value -= damage;

        if (currentLife.Value <= 0)
        {
            // Legion usa respawn con 3 vidas
            if (isLegion)
            {
                // Avisar al script MasterAvatars
                FindAnyObjectByType<MasterAvatars>().LegionDiedServerRpc();
            } else
            {
                // Titan muere de una sin respawn
                DieClientRpc();
            }
        }
    }

    [ClientRpc]
    void DieClientRpc()
    {
        if (IsOwner)
        {
            // Mostrar pantalla de "game over" del titan
            script_UIFlowController.ShowGameOver();
        }
    }
}
