using Unity.Netcode;
using UnityEngine;

public class BonesSync : NetworkBehaviour
{
    public Transform bone;

    private void Update()
    {
        if (!IsOwner) return;
        SendBoneTransform_ServerRPC(bone.transform.position, bone.transform.rotation );
    }
    [ServerRpc]
    void SendBoneTransform_ServerRPC(Vector3 pos, Quaternion rot)
    {
        ApplyToAll_ClientRPC(pos, rot);
    }
    [ClientRpc]
    void ApplyToAll_ClientRPC(Vector3 pos, Quaternion rot)
    {
        if (IsOwner) return;
        bone.transform.position = pos;
        bone.transform.rotation = rot;
    }
}
