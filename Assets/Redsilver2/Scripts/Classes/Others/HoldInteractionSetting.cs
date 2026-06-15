using RedSilver2.Framework.Interactions.Actions;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Settings
{
    [CreateAssetMenu(fileName = "New Hold Interaction Setting", menuName = "Interaction/Setting/Hold")]
    public sealed class HoldInteractionSetting : InteractionSetting {
        public sealed override void Add(InteractionModule module, InteractionAction action)
        {
            if (module == null || action == null) return;
            HoldInteraction interaction = null; //  GetHoldInteraction(module, action.GetInteractionName());

            if (interaction != null)
            {
                interaction?.Enable();
                //   module?.AddInteraction(interaction);
            }
            else
            {
                //  Add(module, new HoldInteraction(action.GetInteractionName()));
            }
        }

        protected override void Add(InteractionModule module, Interaction interaction)
        {
            if (interaction is HoldInteraction) base.Add(module, interaction);
        }

        public static HoldInteraction GetHoldInteraction(InteractionModule module, string interactionName)
        {
            return GetInteraction(module, interactionName) as HoldInteraction;
        }

        public static HoldInteraction[] GetPressInteractions(InteractionModule module)
        {
            return GetInteractions(module).Where(x => x != null)
                                          .Where(x => x is HoldInteraction).ToArray()
                                          as HoldInteraction[];
        }
    }
}
