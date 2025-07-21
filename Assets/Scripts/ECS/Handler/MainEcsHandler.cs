
using Statement;

public class MainEcsHandler : EcsRunHandler
{ 
    public MainEcsHandler(BattleState state) : base(state)
    { 
    }
    public override EcsRunHandler Clone(BattleState state)
    {
        return new MainEcsHandler(state);
    }
    public override void Init() => _commonSystems?.Init(); 
    public override void Run() => _commonSystems?.Run(); 
    public override void FixedRun() => _commonSystems?.Run();
    public override void Dispose()
    { 
        _commonSystems.Destroy();
        World.Destroy();
        World = null;
    }
} 