using Leopotam.EcsLite;

namespace Client 
{
    public interface IResolveAction 
    {
        void Resolve(EcsWorld world, int entity);
    }
}