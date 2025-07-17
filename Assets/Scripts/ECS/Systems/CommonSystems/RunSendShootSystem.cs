using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MemoryPack;

namespace Client 
{
    sealed class RunSendShootSystem : IEcsRunSystem 
    { 
        readonly EcsFilterInject<Inc<SendShootEvent>> _filter = default;
        readonly EcsPoolInject<SendShootEvent> _shootPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var sendShootComp = ref _shootPool.Value.Get(entity);

                NetworkShootRequestEvent networkShootRequestEvent = new NetworkShootRequestEvent()
                {
                    WeaponIndex = sendShootComp.WeaponIndex,
                    SenderEntityKey = sendShootComp.SenderEntityKey,
                    TargetEntityKey = sendShootComp.TargetEntityKey,
                };

                PhotonRunHandler.Instance.SendRequestShootRPC(MemoryPackSerializer.Serialize(networkShootRequestEvent));
            }
        }
    }
}