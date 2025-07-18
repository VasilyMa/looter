using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunBroadcastStartShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AimComponent, NetworkEntityComponent>, Exc<InShootingComponent>> _filter = default;
        readonly EcsPoolInject<InShootingComponent> _inShootingPool = default;
        readonly EcsPoolInject<SendStartShootingEvent> _sendStartShootingPool = default;
        readonly EcsPoolInject<AimComponent> _aimPool = default;
        readonly EcsPoolInject<NetworkEntityComponent> _networkPool = default;
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var aimComp = ref _aimPool.Value.Get(entity);
                ref var networkComp = ref _networkPool.Value.Get(entity);

                ref var sendShootingComp = ref  _sendStartShootingPool.Value.Add(_world.Value.NewEntity());
                sendShootingComp.TargetEntityKeyID = aimComp.Target.gameObject.name;
                sendShootingComp.SenderEntityKeyID = networkComp.EntityKey;

                _inShootingPool.Value.Add(entity); 
            }
        }
    }
}