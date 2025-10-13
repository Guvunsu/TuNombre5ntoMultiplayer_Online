using Unity.Netcode;
using UnityEngine;


public class MasterAvatars : NetworkBehaviour
{
    private NetworkObject networkObject;
    public GameObject titan;
    public GameObject legion;
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            // TITAN: soy el host si mi ID es 0
            if (networkObject.OwnerClientId == 0)
            {
                legion.SetActive(false);
                titan.SetActive(true);
            }
            //Legion soy el cliente y el Id es
            //>0
            else
            {
                legion.SetActive(true);
                titan.SetActive(false);
            }
        }
    }
}
