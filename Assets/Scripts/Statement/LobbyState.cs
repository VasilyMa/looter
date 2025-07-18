 
namespace Statement
{
    public class LobbyState : State
    {
        public override void Start()
        { 
            if (UIModule.OpenCanvas<LobbyCanvas>(out var canvas))
            {
                canvas.OpenPanel<LobbyPanel>();
            }
        }

        public override void Update()
        { 
        }
        public override void FixedUpdate()
        { 
        }

    }
}