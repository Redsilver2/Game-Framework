
#if UNITY_EDITOR
using UnityEditor;

namespace RedSilver2.Framework.StateMachines.EditorTools
{
    [CustomEditor(typeof(PlayerCharacterControllerStateMachine))]
    public sealed class PlayerMovementStateMachineEditor : StateMachineEditor
    {

    }
}
#endif