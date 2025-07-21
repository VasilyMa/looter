using Leopotam.EcsLite;

namespace Client 
{
    public interface IPreRequestAction 
    {
        bool PreRequest(EcsWorld world, int entity);       
    }
}