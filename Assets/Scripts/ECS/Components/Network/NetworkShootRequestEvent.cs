using Leopotam.EcsLite;
using MemoryPack;
using Statement;
using UnityEngine;

namespace Client 
{
    [MemoryPackable]
    public partial struct NetworkShootRequestEvent : IRequestable
    {
        public string SenderEntityKey;
        public string TargetEntityKey;

        public void Request(EcsWorld world)
        {
            var state = BattleState.Instance;

            if (state.TryGetEntity(SenderEntityKey, out int senderEntity) && state.TryGetEntity(TargetEntityKey, out int targetEntity))
            {
                Debug.Log($"Shoot from {senderEntity} to {targetEntity}");
            }
        }
    }
}