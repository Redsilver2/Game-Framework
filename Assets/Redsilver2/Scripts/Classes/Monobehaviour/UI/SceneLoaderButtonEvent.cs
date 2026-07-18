using RedSilver2.Framework.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.UI
{
    public class SceneLoaderButtonEvent : ButtonUISelectionEvent
    {
        [SerializeField] private uint sceneIndex;

#if UNITY_EDITOR
        private void OnValidate()
        {
            sceneIndex = (uint)Mathf.Clamp(sceneIndex, 0, SceneManager.sceneCountInBuildSettings - 1);
        }
#endif

        protected override void SetEvents(ButtonUISelection selection, bool isAddingEvent)
        {
            if (isAddingEvent) selection?.AddOnClickListener(OnClicked);
            else selection?.RemoveOnClickListener(OnClicked);
        }

        private void OnClicked()
        {
            GameManager.SceneLoaderManager?.LoadScene(sceneIndex);
        }
    }
}
