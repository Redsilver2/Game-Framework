using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.UI
{
    public class ClickLoadSceneEvent :  UISelectionButtonOnClickEvent
    {
        [SerializeField] private uint sceneIndex;

#if UNITY_EDITOR
        private void OnValidate()
        {
            sceneIndex = (uint)Mathf.Clamp(sceneIndex, 0, SceneManager.sceneCountInBuildSettings - 1);
        }
#endif

        protected sealed override void OnClick() {
            GameManager.SceneLoaderManager?.LoadScene(sceneIndex);
        }
    }
}
