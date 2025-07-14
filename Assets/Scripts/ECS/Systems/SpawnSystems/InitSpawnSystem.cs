using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Linq;
using UnityEngine;

namespace Client 
{
    sealed class InitSpawnSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<SpawnComponent> _spawnPool = default;

        public void Init (IEcsSystems systems) 
        {
            var entity = _world.Value.NewEntity();

            ref var spawnComp = ref _spawnPool.Value.Add(entity);

            Transform[] allObjects = GameObject.FindObjectsOfType<Transform>();
            int spawnLayer = LayerMask.NameToLayer("SpawnPoint");

            Transform[] spawnPoints = allObjects
                .Where(transform => transform.gameObject.layer == spawnLayer)
                .ToArray();

            spawnComp.SpawnPoints = spawnPoints;
        }
    }
}