using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    [SerializeField] private DoorStateMachine door;
    // Update is called once per frame
    void Update()
    {
        if(InputManager.GetKeyDown(KeyboardKey.F) && door != null) {
            if (door.IsOpen) door?.Close();
            else             door?.Open();
        }
        
    }
}
