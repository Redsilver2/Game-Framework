using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using System;
using UnityEngine;

public abstract class Vector2MouseConfigurationEvent : Vector2InputConfigurationEvent
{
    [Space]
    [SerializeField] private MouseVector2InputSettings settings;

    protected sealed override Vector2InputConfiguration GetConfiguration()
    {
        if (settings == null) return null;
        return settings.GetConfiguration();
    }
}
