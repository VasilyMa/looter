using Leopotam.EcsLite;
using MemoryPack;

namespace Client 
{
    [MemoryPackable]
    public partial struct NetworkFinishShootEvent : IRequestable
    {
        public string SenderEntityKey; 

        public void Request(EcsWorld world)
        { 

        }
    }
}