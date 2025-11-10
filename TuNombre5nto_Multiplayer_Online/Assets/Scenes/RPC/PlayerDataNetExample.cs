using JetBrains.Annotations;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public struct PlayerDataNetExample : INetworkSerializable
{
    public int goldCount;
    public Vector3 killPosition;
    public void NetworkSerialize<T>(BufferSerializer<T> seriealizer) where T : IReaderWriter
    {
        seriealizer.SerializeValue(ref goldCount);
        seriealizer.SerializeValue(ref killPosition);
    }
}
