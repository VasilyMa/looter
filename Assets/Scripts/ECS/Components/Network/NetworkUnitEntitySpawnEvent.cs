using Fusion;
using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    [MemoryPack.MemoryPackable]
    public partial struct NetworkUnitEntitySpawnEvent : IRequestable
    {
        public int PlayerOwner;
        public string SpawnKeyID;
        public string EntityKey;

        public void Request(EcsWorld world)
        {
            var unitEntityConfig = ConfigModule.GetConfig<EntityConfig>();

            if (unitEntityConfig.TryGetEntity(SpawnKeyID, out EntityBase entityBase))
            {
                var entity = world.NewEntity();

                entityBase.InitEntity(world, entity);

                ref var networkComp = ref world.GetPool<NetworkEntityComponent>().Add(entity);
                networkComp.EntityKey = EntityKey;
                networkComp.PlayerOwner = PlayerOwner;
                
                if (PhotonRunHandler.Instance.Runner.LocalPlayer.PlayerId == PlayerOwner)
                {
                    world.GetPool<OwnComponent>().Add(entity);

                    BattleState.Instance.PlayerEntity = entity;

                    if (BattleState.Instance.TryGetEntity("camera", out int cameraEntity))
                    {
                        ref var transformComp = ref world.GetPool<TransformComponent>().Get(entity);

                        world.GetPool<CameraSwitchEvent>().Add(cameraEntity).Target = transformComp.Transform;
                    }
                }

                BattleState.Instance.AddEntity(EntityKey, entity);
            }
        }
    }
 
    /// <summary>
    /// This entity is networked
    /// </summary>
    public struct NetworkEntityComponent
    {
        public int PlayerOwner; 
        public string EntityKey;
    } 
}