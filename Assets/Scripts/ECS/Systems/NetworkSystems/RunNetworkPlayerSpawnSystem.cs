using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MemoryPack;

namespace Client 
{
    sealed class RunNetworkPlayerSpawnSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<NetworkPlayerSpawnEvent>> _filter = default;
        readonly EcsPoolInject<NetworkPlayerSpawnEvent> _pool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var networkComp = ref _pool.Value.Get(entity);

                PhotonRunHandler.Instance.SendPlayerSpawnRPC(MemoryPackSerializer.Serialize<NetworkPlayerSpawnEvent>(networkComp));
            }
        }
    }
}