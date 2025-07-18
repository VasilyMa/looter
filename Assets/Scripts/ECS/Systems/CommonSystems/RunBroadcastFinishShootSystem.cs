using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunBroadcastFinishShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InShootingComponent, CancelShootingEvent, NetworkEntityComponent>> _filter = default;
        readonly EcsPoolInject<InShootingComponent> _inShootPool = default;
        readonly EcsPoolInject<SendFinishShootingEvent> _sendFinishPool = default;
        readonly EcsPoolInject<NetworkEntityComponent> _networkPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var networkComp = ref _networkPool.Value.Get(entity);
                ref var sendFinishComp = ref _sendFinishPool.Value.Add(_world.Value.NewEntity());
                sendFinishComp.SenderEntityKey = networkComp.EntityKey;

                _inShootPool.Value.Del(entity);
            }
        }
    }
}