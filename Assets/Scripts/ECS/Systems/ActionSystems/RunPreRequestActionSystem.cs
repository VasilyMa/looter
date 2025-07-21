using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client 
{
    sealed class RunPreRequestActionSystem<T> : IEcsRunSystem where T : struct, IPreRequestAction
    {
        readonly EcsFilterInject<Inc<T, ActionComponent, PreRequestActionEvent>> _filter = default;
        readonly EcsPoolInject<T> _pool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var preRequestComp = ref _pool.Value.Get(entity);
                preRequestComp.PreRequest();
            }
        }
    }
}