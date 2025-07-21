using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunDisposeActionSystem<T> : IEcsRunSystem  where T : struct, IDispose
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ActionComponent, InActionComponent, DisposeActionEvent>> _filter = default;
        readonly EcsPoolInject<ResolveActionEvent> _resolveActionPool = default;
        readonly EcsPoolInject<ActionComponent> _actionPool = default;
        readonly EcsPoolInject<T> _pool = default;  

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var actionComp = ref _actionPool.Value.Get(entity);
                ref var disposeComp = ref _pool.Value.Get(actionComp.CurrentActionEntity);
                disposeComp.Dispose(_world.Value, actionComp.CurrentActionEntity);

                _resolveActionPool.Value.Del(entity);
            }
        }
    }
}