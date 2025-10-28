using UnityEngine;
using Unity.Netcode;
public class PaintPlayers : NetworkBehaviour
{
    /// <summary>
    /// hacer Utilizando la información que otorga unity sobre ID de usuarios a través de la red, deberás pintar al host de rojo y al cliente de azul.
    /// </summary>
    Renderer renderObj;

    void Start()
    {
        renderObj = GetComponent<Renderer>();
        if (IsOwner)
        {
            if (IsHost)
                SetColor(Color.red);
            else if (IsClient)
                SetColor(Color.blue);
        }
    }

    void SetColor(Color c)
    {
        renderObj.material.color = c;
    }
}
