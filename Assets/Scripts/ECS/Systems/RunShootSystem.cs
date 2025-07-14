using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunShootSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AimComponent>> _filter = default;
        readonly EcsPoolInject<AimComponent> _aimPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var aimComp = ref _aimPool.Value.Get(entity);
                //todo shoot
            }
        }
    }
}