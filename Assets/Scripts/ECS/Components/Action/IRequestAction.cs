using Leopotam.EcsLite;

namespace Client 
{
    public interface IRequestAction
    {
        bool Request(EcsWorld world, int entity);
    }
}