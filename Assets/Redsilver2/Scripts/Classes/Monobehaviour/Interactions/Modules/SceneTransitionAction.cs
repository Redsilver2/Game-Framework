using RedSilver2.Framework.Scenes;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions
{
    [System.Serializable]
    public abstract class SceneTransitionAction : InteractionAction
    {
        [Space]
        [SerializeField] private bool isLoadingSingleScene;

        protected SceneTransitionAction(InteractionModule module, Interaction interaction) : base(module, interaction) {
            interaction?.AddOnInteractedListener(handler => { TransitionScene(GameManager.SceneLoaderManager, isLoadingSingleScene); });
        }

        protected abstract void TransitionScene(SceneLoaderManager manager, bool isLoadingSingleScene);
    }
}