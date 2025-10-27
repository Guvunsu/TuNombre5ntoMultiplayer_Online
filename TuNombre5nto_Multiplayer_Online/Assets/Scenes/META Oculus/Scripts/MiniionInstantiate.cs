using Unity.Netcode;
using UnityEngine;

public class MiniionInstantiate : NetworkBehaviour
{
    [Header("Minion")]
    public NetworkObject miniTitan;
    public float speedMov = 20f;

    Health script_Health; // hay que llmar la referencia para bajar vida al explotar

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void InstantiateMinion()
    {

    }
    public void ExplotingMinion()
    {

    }
}
