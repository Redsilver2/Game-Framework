
using RedSilver2.Framework.Interactions.Actions;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Settings
{
    public sealed class ReleaseInteractionSetting : InteractionSetting
    {
        public override void Add(InteractionModule module, InteractionAction action) {

        }

        protected override void Add(InteractionModule module, Interaction interaction) {
            if (interaction is HoldInteraction) base.Add(module, interaction);
        }

        public static ReleaseInteraction GetReleaseInteraction(InteractionModule module, string interactionName)
        {
            return GetInteraction(module, interactionName) as ReleaseInteraction;
        }

        public static ReleaseInteraction[] GetPressInteractions(InteractionModule module)
        {
            return GetInteractions(module).Where(x => x != null)
                                          .Where(x => x is ReleaseInteraction).ToArray()
                                          as ReleaseInteraction[];
        }
    }
}
