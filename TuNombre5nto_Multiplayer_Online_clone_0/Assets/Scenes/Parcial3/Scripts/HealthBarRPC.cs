using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarRPC : MonoBehaviour
{
    //ponerle una camara a la cabeza del titan, cambiar la logic de mi masteravatr en mi prefab para que sea mi hosttitan mi clienttitan
    [Header("Barra De Vida")]
    public Image imageHealthBar;
    public float currentLife = 100f;

    [Header("referencias")]
    public UIFlowController script_UIFlowController;

    void Start()
    {
        currentLife = 100f;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("MiniTitan"))
        { 
            Debug.Log($"Me hizo daño el desgraciou" + collision.gameObject);
            TakingDamageRPC();
        }
    }
    [ServerRpc]
    public void TakingDamageRPC()
    {
        currentLife -= 10f;
        if (currentLife <= 0)
        {
            script_UIFlowController.ShowGameOver();
        }
    }
}
