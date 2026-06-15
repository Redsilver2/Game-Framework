using RedSilver2.Framework.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Interactions.Actions
{

    [System.Serializable]
    public sealed class IndexSceneTransitionAction : SceneTransitionAction
    {
        [Space]
        [SerializeField] private uint sceneIndex;

        public IndexSceneTransitionAction(InteractionModule module, Interaction interaction) : base(module, interaction) {
       
        }

        protected override void TransitionScene(SceneLoaderManager manager, bool isLoadingSingleScene) {
            if (isLoadingSingleScene) manager?.LoadSingleScene(sceneIndex);
            else manager?.LoadScene(sceneIndex);
        }
    }
}
