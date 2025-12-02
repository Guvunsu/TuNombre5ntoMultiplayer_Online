using Unity.Netcode;
using UnityEngine;

public struct PlayerDataNet : INetworkSerializable
{
    public int goldCount;
    public Vector3 killPosition;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref goldCount);
        serializer.SerializeValue(ref killPosition);
    }
}
