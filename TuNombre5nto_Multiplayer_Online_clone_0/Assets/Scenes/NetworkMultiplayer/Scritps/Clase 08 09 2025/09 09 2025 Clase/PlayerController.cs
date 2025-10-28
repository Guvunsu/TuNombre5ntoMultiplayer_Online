using UnityEngine;
using Unity.Netcode;
public class PlayerController : NetworkBehaviour
{
    public float speed = 15;
    void Start()
    {

    }
    void Update()
    {
        // only let the owning client move its own player
        if (!IsOwner) return;
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 input = new Vector3(h, 0, v);
            transform.Translate(input * speed * Time.deltaTime);
        }
    }
}
