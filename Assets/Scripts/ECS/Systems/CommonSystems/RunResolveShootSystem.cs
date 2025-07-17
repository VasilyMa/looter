using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunResolveShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ResolveShootEvent, WeaponComponent>> _filter = default;
        readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        readonly EcsPoolInject<SendShootEvent> _sendPool = default;
        readonly EcsPoolInject<ResolveShootEvent> _resolvePool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var weaponComp = ref _weaponPool.Value.Get(entity);
                ref var resovleComp = ref _resolvePool.Value.Get(entity);
                ref var sendComp = ref _sendPool.Value.Add(_world.Value.NewEntity());

                sendComp.WeaponIndex = weaponComp.Index;
                sendComp.SenderEntityKey = weaponComp.OwnerEntityKey;
                sendComp.TargetEntityKey = resovleComp.TargetKey;
            }
        }
    }
}