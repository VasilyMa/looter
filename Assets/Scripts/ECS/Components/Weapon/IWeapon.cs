using Leopotam.EcsLite; 
public interface IWeapon
{
    void InitWeapon(EcsWorld world, int entity, string ownerEntity);
}
