//using UnityEngine;
//using Unity.Netcode;
//public class Shooting : NetworkBehaviour
//{
//    #region enum
//    public enum player_Enum
//    {
//        NO_SHOOTING,
//        SHOOTING
//    }
//    #endregion enum

//    #region Variables
//    [Header("Object Sphere Player")]
//    [SerializeField] Transform sphere_Controller;

//    public player_Enum enum_Player;
//    [Header("Variables Bullet")]
//    [SerializeField] float bullet_velocity = 100f;

//    [Header("Bullet i will shoot")]
//    [SerializeField] NetworkObject bullet_Prefab;
//    #endregion Variables

//    #region Unity Methods
//    void Update()
//    {
//        if (!IsOwner) return;
//        switch (enum_Player)
//        {
//            case player_Enum.NO_SHOOTING:
//                break;
//            case player_Enum.SHOOTING:
//                ShootingPlayer();
//                break;
//        }
//    }
//    #endregion UnityMethods
//    #region PublicMethods
//    void ShootingPlayer()
//    {
//        if (Input.GetKeyUp(KeyCode.Escape))
//        {
//            bullet_velocity = Time.fixedDeltaTime;
//            Vector3 pos = transform.position + transform.forward;
//            Quaternion rot = Quaternion.LookRotation(transform.forward);
//            var proj = Instantiate(bullet_Prefab, pos, rot);
//            var simple = proj.GetComponent<NetworketProjectile>();
//            proj.Spawn;
//            enum_Player = player_Enum.SHOOTING;
//        }
//    }
//    #endregion PublicMethods
//    [ServerRpc]
//    public void SendBullet_ServerRPC(Vector3 pos, Quaternion rot)
//    {
//        NetworkObject bulletInstantiate = Instantiate(bullet_Prefab, pos, rot);
//        bulletInstantiate.Spawn();
//        Rigidbody rb = bulletInstantiate.GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            rb.linearVelocity = rot * Vector3.forward * bullet_velocity;
//        }
//    }
//}
