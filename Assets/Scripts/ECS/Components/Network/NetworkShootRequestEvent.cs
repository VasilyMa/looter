using Leopotam.EcsLite;
using MemoryPack;
using Statement;
using UnityEngine;

namespace Client 
{
    [MemoryPackable]
    public partial struct NetworkShootRequestEvent : IRequestable
    {
        public int WeaponIndex;
        public string SenderEntityKey;
        public string TargetEntityKey;

        public void Request(EcsWorld world)
        {
            var state = BattleState.Instance;

            if (state.TryGetEntity(SenderEntityKey, out int senderEntity) && state.TryGetEntity(TargetEntityKey, out int targetEntity))
            {
                var hitPool = world.GetPool<HitRequest>();

                if (!hitPool.Has(targetEntity)) hitPool.Add(targetEntity);

                ref var hitComp = ref hitPool.Get(targetEntity);
                hitComp.SenderEntity = senderEntity;
                hitComp.WeaponIndex = WeaponIndex; 

                Debug.Log($"Shoot from {senderEntity} to {targetEntity}");
            }
        }
    }

    public struct HitRequest
    {
        public int WeaponIndex;
        public int SenderEntity;
    }
}