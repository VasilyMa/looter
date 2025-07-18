using Fusion;

namespace Statement
{
    public class MainMenuState : State
    {
        public static new MainMenuState Instance
        {
            get
            {
                return (MainMenuState)State.Instance;
            }
        }
        public override void FixedUpdate()
        { 
        }

        public override void Start()
        { 
            if (UIModule.OpenCanvas<MainMenuCanvas>(out var canvas))
            {
                canvas.OpenPanel<MainMenuPanel>();
            }
        }

        public override void Update()
        { 

        }

        public async void StartMatchmaking()
        {
            var session = new SessionParams
            {
                Mode = GameMode.AutoHostOrClient,
                RoomName = $"MatchMaking",
                ScenePath = "battle_scene_01",
                SceneIndex = 2,
                ProvideInput = true,
                TargetPlayerCount = 1
            };

            await PhotonInitializer.Instance.StartSession(session);
        }

        async void TutorialStart()
        { 
            var session = new SessionParams
            {
                Mode = GameMode.Single,
                RoomName = "TutorialRoom",

                ProvideInput = true 
            };

            await PhotonInitializer.Instance.StartSession(session);
        }
    }
}