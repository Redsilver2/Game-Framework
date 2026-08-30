
namespace RedSilver2.Framework.StateMachines.States
{
    [System.Serializable]
    public sealed class Idol : Movement
    {
        public IdolState BaseState => GetState() as IdolState;
        public Idol() {  }

        protected sealed override void SetBaseState(ref MovementState state)
        {
            state = new IdolState();
        }
    }
}
