using System.Collections.Generic;

using Leopotam.EcsLite;

namespace Statement
{
    public class OnlineState : BattleState
    {

        public static new OnlineState Instance
        {
            get
            {
                return (OnlineState)State.Instance;
            }
        }
        public override void Awake()
        {
            base.Awake();

        }

    }
}