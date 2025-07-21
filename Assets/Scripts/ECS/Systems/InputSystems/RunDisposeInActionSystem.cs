using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunDisposeInActionSystem : IEcsRunSystem
    { 
        readonly EcsFilterInject<Inc<ActionComponent, InActionComponent, DisposeActionEvent>> _filter = default;
        readonly EcsPoolInject<InActionComponent> _inActionPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                _inActionPool.Value.Del(entity);
            }
        }
    }
}