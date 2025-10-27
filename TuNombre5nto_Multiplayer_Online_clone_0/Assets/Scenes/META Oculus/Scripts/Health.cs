using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    [Header("Health")]
    [SerializeField] int healthMinum = 0;
    [SerializeField] int healthMaximun = 100;
    [SerializeField] int currentHealth = 100;
    public Slider healthSlider;

    [Header("Referencias Titan")]
    [SerializeField] NetworkObject titan;

    [Header("Referencias Aliado")]
    [SerializeField] NetworkObject aliado;

    void Start()
    {

    }
    void Update()
    {

    }
    public void HealthBarNetObjRPC(int health)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = healthMaximun;
            healthSlider.value = health;
        }
    }
    public void DamageTakeItRPC(int amount)
    {
        if (!IsClient) return;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, healthMaximun);
    }
  
}
