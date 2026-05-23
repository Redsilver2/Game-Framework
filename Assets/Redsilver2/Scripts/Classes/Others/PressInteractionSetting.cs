using RedSilver2.Framework.Interactions.Actions;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Settings
{
    [CreateAssetMenu(fileName = "New Press Interaction Setting", menuName = "Interaction/Setting/Press")]
    public sealed class PressInteractionSetting : InteractionSetting
    {
        public sealed override void Add(InteractionModule module, InteractionAction action) {
            if (module == null || action == null) return;
            PressInteraction interaction = GetPressInteraction(module, action.GetInteractionName());

            if (interaction != null) {
                interaction?.Enable();
                module?.AddInteraction(interaction);
            }
            else {
                Add(module, new PressInteraction(action.GetInteractionName()));        
            }
        }

        protected override void Add(InteractionModule module, Interaction interaction) {
            if(interaction is PressInteraction) base.Add(module, interaction);
        }

        public static PressInteraction GetPressInteraction(InteractionModule module, string interactionName) {
            return GetInteraction(module, interactionName) as PressInteraction; 
        }

        public static PressInteraction[] GetPressInteractions(InteractionModule module) {
            return GetInteractions(module).Where(x => x != null)
                                          .Where(x => x is PressInteraction).ToArray() 
                                          as PressInteraction[]; 
        }
    }
}
