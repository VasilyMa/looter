using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunResolveActionSystem<T> : IEcsRunSystem where T : struct, IResolveAction 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<T, ActionComponent, ResolveActionEvent>> _filter = default;
        readonly EcsPoolInject<T> _pool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var resolveComp = ref _pool.Value.Get(entity);
                resolveComp.Resolve();
            }
        }
    }
}