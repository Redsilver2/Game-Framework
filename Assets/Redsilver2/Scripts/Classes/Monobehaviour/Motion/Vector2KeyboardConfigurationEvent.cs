using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Vector2KeyboardConfigurationEvent : Vector2InputConfigurationEvent
{
    [SerializeField] private KeyboardVector2InputSettings settings;

    protected override Vector2InputConfiguration GetConfiguration()
    {
        if(settings == null) return null;
        var configuration = settings.GetConfiguration();
        return configuration;
    }
}
