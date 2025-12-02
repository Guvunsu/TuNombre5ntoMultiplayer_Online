using UnityEngine;
using Unity.Netcode;

public class NetworkSceneLoader : MonoBehaviour
{
    public GameObject panelLoading;

    private void OnEnable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        } else
        {
            Debug.LogWarning("[NetworkSceneLoader] NetworkManager.Singleton es null en OnEnable. Asegúrate de que NetworkManager exista en la escena y se inicialice antes.");
            // Intento de reintento simple:
            StartCoroutine(DelayedSubscribe());
        }
    }

    private System.Collections.IEnumerator DelayedSubscribe()
    {
        float timeout = 5f;
        float t = 0f;
        while (NetworkManager.Singleton == null && t < timeout)
        {
            yield return null;
            t += Time.deltaTime;
        }
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        else
            Debug.LogError("[NetworkSceneLoader] No se encontró NetworkManager.Singleton después del timeout.");
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        // Sólo ocultar el panel loading para el cliente local cuando SU conexión se confirma.
        if (NetworkManager.Singleton == null) return;

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            if (panelLoading != null)
                panelLoading.SetActive(false);
        }
    }
}
