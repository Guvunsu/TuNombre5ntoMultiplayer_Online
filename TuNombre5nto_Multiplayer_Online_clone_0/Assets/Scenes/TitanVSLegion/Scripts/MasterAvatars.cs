using Unity.Netcode;
using UnityEngine;


public class MasterAvatars : NetworkBehaviour
{
    private NetworkObject networkObject;
    public GameObject HostTitan, ClientTitan;
    public GameObject HostLegion, ClientLegion;
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            bool isOwner = networkObject.IsOwner;
            // TITAN: soy el host si mi ID es 0
            if (networkObject.OwnerClientId == 0)
            {
                if (isOwner) HostTitan.SetActive(true);
                else ClientTitan.SetActive(true);
            }
            //Legion soy el cliente y el Id es >0
            else
            {
                if (isOwner) ClientLegion.SetActive(true);
                else HostLegion.SetActive(true);
            }
        }
    }
}
