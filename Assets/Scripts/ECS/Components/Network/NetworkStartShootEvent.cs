using Leopotam.EcsLite;
using MemoryPack;
using Statement;

namespace Client 
{
    [MemoryPackable]
    public partial struct NetworkStartShootEvent : IRequestable
    {
        public string SenderEntityKeyID;
        public string TargetEntityKeyID; 

        public void Request(EcsWorld world)
        {
            var state = BattleState.Instance;

            if (state.TryGetEntity(SenderEntityKeyID, out int senderEntity) && state.TryGetEntity(TargetEntityKeyID, out int targetEntity))
            {

            }
        }
    }
}