using Leopotam.EcsLite;
using MemoryPack;
using Statement;
using UnityEngine;

namespace Client 
{
    [MemoryPackable]
    public partial struct NetworkTransformEvent : IRequestable
    {
        public string EntityKey;
        public Vector3 Position;
        public Quaternion Rotation;

        public void Request(EcsWorld world)
        {
            var state = BattleState.Instance;

            if (state.TryGetEntity(EntityKey, out int entity))
            {
                if (!world.GetPool<ReceiveTransformEvent>().Has(entity))
                {
                    world.GetPool<ReceiveTransformEvent>().Add(entity);
                }

                ref var networkComp = ref world.GetPool<ReceiveTransformEvent>().Get(entity);
                networkComp.Position = Position;
                networkComp.Quaternion = Rotation;
            }
        }
    }

    public struct ReceiveTransformEvent
    {
        public Vector3 Position;
        public Quaternion Quaternion;
    }
}