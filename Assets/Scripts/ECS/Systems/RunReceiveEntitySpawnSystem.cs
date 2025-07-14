using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using Unity.Collections.LowLevel.Unsafe;

namespace Client 
{
    sealed class RunReceiveEntitySpawnSystem : IEcsRunSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<ReceiveSpawnEvent>> _filter = default;
        readonly EcsPoolInject<ReceiveSpawnEvent> _receiveSpawnPool = default;
        readonly EcsPoolInject<NetworkEntityComponent> _networkEntityPool = default;
        readonly EcsPoolInject<OwnComponent> _ownPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<CameraSwitchEvent> _camerPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (IEcsSystems systems) 
        {
            foreach (var e in _filter.Value)
            {
                ref var receiveSpawnComp = ref _receiveSpawnPool.Value.Get(e);

                var unitEntityConfig = ConfigModule.GetConfig<EntityConfig>();

                if (unitEntityConfig.TryGetEntity(receiveSpawnComp.SpawnKeyID, out EntityBase entityBase))
                {
                    var entity = _world.Value.NewEntity();

                    entityBase.InitEntity(_world.Value, entity);

                    ref var networkComp = ref _networkEntityPool.Value.Add(entity);
                    networkComp.EntityKey = receiveSpawnComp.EntityKey;
                    networkComp.PlayerOwner = receiveSpawnComp.PlayerOwner;

                    ref var viewComp = ref _viewPool.Value.Add(entity);
                    viewComp.RefObject.name = networkComp.EntityKey;

                    if (PhotonRunHandler.Instance.Runner.LocalPlayer.PlayerId == receiveSpawnComp.PlayerOwner)
                    {
                        _ownPool.Value.Add(entity);

                        BattleState.Instance.PlayerEntity = entity;

                        if (BattleState.Instance.TryGetEntity("camera", out int cameraEntity))
                        {
                            ref var transformComp = ref _transformPool.Value.Add(entity);

                            _camerPool.Value.Add(cameraEntity).Target = transformComp.Transform;
                        }
                    }

                    BattleState.Instance.AddEntity(receiveSpawnComp.EntityKey, entity);
                }
            }
        }
    }
}