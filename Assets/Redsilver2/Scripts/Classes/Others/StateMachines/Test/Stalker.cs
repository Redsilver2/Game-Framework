using RedSilver2.Framework.StateMachines;
using RedSilver2.Framework.StateMachines.Controllers;

public class Stalker : AIMovementController
{
    public override AIMovementStateMachine GetMovementStateMachine()
    {
        return new AIMovementStateMachine(this);
    }
}
