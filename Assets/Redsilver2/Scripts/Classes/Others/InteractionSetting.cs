
using RedSilver2.Framework.Interactions.Actions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Settings
{
    public abstract class InteractionSetting : ScriptableObject
    {
        private readonly static Dictionary<InteractionModule, List<Interaction>> instances = new Dictionary<InteractionModule, List<Interaction>>();

        protected virtual void Add(InteractionModule module, Interaction interaction)
        {
            if (module == null || interaction == null || instances == null) 
                return;
  
            if (!instances.ContainsKey(module)) {
                instances.Add(module, new List<Interaction>());
            }

            if (!instances[module].Contains(interaction)) { 
              //  module?.AddInteraction(interaction);
                instances[module].Add(interaction);
            }
        }

        public void Remove(InteractionModule module, InteractionAction action)
        {
            if(module == null || action == null || instances == null || !instances.ContainsKey(module))
                return;

          //  Remove(module, action.GetInteractionName());
        }

        public void Remove(InteractionModule module, string interactionName)
        {
            if (module == null || instances == null || !instances.ContainsKey(module) || string.IsNullOrEmpty(interactionName))
                return;

            Remove(module, GetInteraction(module, interactionName));       
        }

        public void Remove(InteractionModule module, Interaction interaction) {
            if (module == null || interaction == null || instances == null || !instances.ContainsKey(module))
                return;

            if (instances[module].Contains(interaction)) {
             //   module?.RemoveInteraction(interaction);
                interaction?.Disable();
            }
        }

        public bool Contains(InteractionModule module)
        {
            if (instances == null || module == null) return false;
            return instances.ContainsKey(module);
        }

        public abstract void Add(InteractionModule module, InteractionAction action);
        public static Interaction GetInteraction(InteractionModule module, InteractionAction action)
        {
            if(module == null || action == null) return null;
            return null;
          //  return GetInteraction(module, action.GetInteractionName());
        }

        public static Interaction GetInteraction(InteractionModule module, string interactionName) {
            if(module == null || string.IsNullOrEmpty(interactionName)) return null;

            var results = GetInteractions(module).Where(x => x != null)
                                                 .Where(x => x.Name.ToLower().Equals(interactionName.ToLower()));

            return results.Count() > 0 ? results.First() : null;           
        }

        public static Interaction[] GetInteractions(InteractionModule module)
        {
            if (module == null || instances == null || !instances.ContainsKey(module))
                return new Interaction[0];

            return instances[module].ToArray(); 
        }
    }
}
