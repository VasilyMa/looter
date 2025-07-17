using Leopotam.EcsLite;

namespace Client 
{
    struct ReloadComponent : IWeapon
    {
        public float ReloadTime;
        [UnityEngine.HideInInspector] public float CurrentReloadTime;

        public void InitWeapon(EcsWorld world, int entity, string ownerEntity)
        {
            ref var reloadComp = ref world.GetPool<ReloadComponent>().Add(entity);
            reloadComp.CurrentReloadTime = ReloadTime;
            reloadComp.ReloadTime = ReloadTime;
        }
    }
}