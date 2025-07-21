using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunRequestActionSystem<T> : IEcsRunSystem where T : struct, IRequestAction
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ActionComponent, RequestActionEvent, InActionComponent>> _filter = default;
        readonly EcsPoolInject<T> _pool = default;
        readonly EcsPoolInject<ActionComponent> _actionPool = default;
        readonly EcsPoolInject<ResolveActionEvent> _resolvePool = default;
        
        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var actionComp = ref _actionPool.Value.Get(entity);
                ref var requestComp = ref _pool.Value.Get(actionComp.CurrentActionEntity);
                if (requestComp.Request(_world.Value, actionComp.CurrentActionEntity)) _resolvePool.Value.Add(entity);
            }
        }
    }
}