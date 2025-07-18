using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MemoryPack;

namespace Client 
{
    sealed class RunSendStartShootingSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<SendStartShootingEvent>> _filter = default;
        readonly EcsPoolInject<SendStartShootingEvent> _sendShootingPool = default; 

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var sendShootComp = ref _sendShootingPool.Value.Get(entity);

                NetworkStartShootEvent networkStartShootEvent = new NetworkStartShootEvent()
                {
                    SenderEntityKeyID = sendShootComp.SenderEntityKeyID,
                    TargetEntityKeyID = sendShootComp.TargetEntityKeyID,
                };

                PhotonRunHandler.Instance.SendRequestStartShootingRPC(MemoryPackSerializer.Serialize(networkStartShootEvent));
            }
        }
    }
}