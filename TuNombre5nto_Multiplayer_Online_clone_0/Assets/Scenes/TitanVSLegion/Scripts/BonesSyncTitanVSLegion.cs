using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class BonesSyncTitanVSLegion : NetworkBehaviour
{
    public Transform[] bones;
    public Transform[] targets;
    public TransformData[] transformData;
    //public Transform bone;

    private void Start()
    {
        transformData = new TransformData[bones.Length];
    }
    private void Update()
    {
        if (!IsOwner) return;
        for (int i = 0; i < bones.Length; i++)
        {
            transformData[i].position = bones[i].position;
            transformData[i].rotation = bones[i].rotation;
        }
        SendBonesArray_ServerRPC(transformData);

        //SendBoneTransform_ServerRPC(bone.transform.position, bone.transform.rotation);
    }
    // esto no lo necesito porque es copy paste de otra clase y no lo ocupamos para esta scene :3
    /*  //[ServerRpc]
      //void SendBoneTransform_ServerRPC(Vector3 pos, Quaternion rot)
      //{
      //    ApplyToAll_ClientRPC(pos, rot);
      //}*/
    /*  //[ClientRpc]
      //void ApplyToAll_ClientRPC(Vector3 pos, Quaternion rot)
      //{
      //    if (IsOwner) return;
      //    bone.transform.position = pos;
      //    bone.transform.rotation = rot;
      //} */
    [ServerRpc]
    void SendBonesArray_ServerRPC(TransformData[] data)
    {
        ApplyTransformArrayToAll_ClientRPC(data);
    }
    [ClientRpc]
    void ApplyTransformArrayToAll_ClientRPC(TransformData[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            targets[i].position = data[i].position;
            targets[i].rotation = data[i].rotation;
        }
    }
}
