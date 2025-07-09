using Leopotam.EcsLite;
using Statement;

namespace Client 
{
    [MemoryPack.MemoryPackable]
    public partial struct NetworkUnitEntitySpawnEvent : IRequestable
    {
        public string SpawnKeyID;
        public string EntityKey;

        public void Request(EcsWorld world)
        {
            var unitEntityConfig = ConfigModule.GetConfig<EntityConfig>();

            if (unitEntityConfig.TryGetEntity(SpawnKeyID, out EntityBase entityBase))
            {
                var entity = world.NewEntity();

                entityBase.InitEntity(world, entity);

                BattleState.Instance.AddEntity(EntityKey, entity);
            }
        }
    }
 
    public struct RequestEvent
    {

    }
}