using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunResolveActionSystem<T> : IEcsRunSystem where T : struct, IResolveAction 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ActionComponent, ResolveActionEvent, InActionComponent>, Exc<DisposeActionEvent>> _filter = default;
        readonly EcsPoolInject<T> _pool = default;
        readonly EcsPoolInject<ActionComponent> _actionPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var actionComp = ref _actionPool.Value.Get(entity);
                ref var resolveComp = ref _pool.Value.Get(actionComp.CurrentActionEntity);
                resolveComp.Resolve(_world.Value, actionComp.CurrentActionEntity);
            }
        }
    }
}