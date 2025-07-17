using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statement
{
    public class CreateCharacterState : State
    {
        public static new CreateCharacterState Instance
        {
            get
            {
                return (CreateCharacterState)State.Instance;
            }
        }
        public List<CharacterBase> PlayerAvaiableCharacters;

        public override void Start()
        {
            //InvokeCanvas<CreateCharacterCanvas>().OpenPanel<CreateCharacterPanel>();
        }

        public override void FixedUpdate()
        {

        }

        public override void Update()
        {

        }
    }
}