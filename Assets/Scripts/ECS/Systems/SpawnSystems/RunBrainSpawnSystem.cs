using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class RunBrainSpawnSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<SpawnComponent>> _filter = default;
        readonly EcsPoolInject<SpawnComponent> _spawnPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var spawnComp = ref _spawnPool.Value.Get(entity);

                spawnComp.SpawnTimeRemaining -= Time.deltaTime;

                if (spawnComp.SpawnTimeRemaining <= 0)
                {
                    spawnComp.SpawnTimeRemaining = spawnComp.SpawnTick;

                    int random = Random.Range(0, spawnComp.SpawnPoints.Length);

                    Vector3 spawnPos = spawnComp.SpawnPoints[random].position;


                }
            }
        }
    }
}