using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MemoryPack;

namespace Client 
{
    sealed class RunSendFinishShootingSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<SendFinishShootingEvent>> _filter = default;
        readonly EcsPoolInject<SendFinishShootingEvent> _sendFinishPool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var senderFinishComp = ref _sendFinishPool.Value.Get(entity);

                NetworkFinishShootEvent networkStartShootEvent = new NetworkFinishShootEvent()
                {
                    SenderEntityKey = senderFinishComp.SenderEntityKey, 
                };

                PhotonRunHandler.Instance.SendRequestFinishShootingRPC(MemoryPackSerializer.Serialize(networkStartShootEvent));
            }
        }
    }
}