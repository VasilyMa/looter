using Client;
using Statement;

public class TutorEcsHandler : EcsRunHandler
{
    public TutorEcsHandler(BattleState state) : base(state) 
    { 
    } 

    public override EcsRunHandler Clone(BattleState state)
    {
        return new TutorEcsHandler(state);
    }
}
