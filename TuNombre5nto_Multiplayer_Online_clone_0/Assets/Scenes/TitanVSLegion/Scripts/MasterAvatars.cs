using Unity.Netcode;
using UnityEngine;


public class MasterAvatars : NetworkBehaviour
{
    private NetworkObject networkObject;
    // quitar el comentado para que sirva parcial 2, haci solo comentado sirve para el parcial 3
    public GameObject HostTitan, ClientTitan;
    public GameObject HostLegion, ClientLegion;
    //public GameObject boxCollider;
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            bool isOwner = networkObject.IsOwner;
        //TITAN: soy el host si mi ID es 0
            if (networkObject.OwnerClientId == 0)
            {
                if (isOwner) HostTitan.SetActive(true);
                //escribir el transform de donde aparecera el titan
                else ClientTitan.SetActive(true); //parcial 2
                //else ClientLegion.SetActive(true); //parcial 3
                //escribir el transform de donde aparecera la legion
            }
            //parcial 2 hay que descomentarlo para que funcione
            //Legion soy el cliente y el Id es > 0
            else
            {
                if (isOwner) ClientLegion.SetActive(true);
                else HostLegion.SetActive(true);

            }
        }
    }
}
