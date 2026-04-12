using RedSilver2.Framework.Inputs.Settings;
using UnityEngine;

public class InputSettingsInitializer : ScriptableObject 
{
    [SerializeField] private InputSettings[] settings;

    public void Enable()
    {
        foreach(var setting in settings)
            setting?.Enable();
    }

    public void Disable()
    {
        foreach(var setting in settings)
            setting?.Disable(); 
    }
}
