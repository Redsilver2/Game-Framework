using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public abstract partial class SceneLoaderManager : MonoBehaviour
    {
        public sealed class TestScene01 : SceneData
        {
            private TestScene01() {   }

            public TestScene01(int sceneIndex, bool isUnlocked) : base(sceneIndex, isUnlocked)
            {

            }

            protected override void SetSceneLoaderManagerEvents(SceneLoaderManager sceneLoaderManager)
            {
                base.SetSceneLoaderManagerEvents(sceneLoaderManager);
            }

            protected sealed override void SetName(out string sceneName)
            {
                sceneName = "Test Scene 01";
            }

            public override string GetDescription()
            {
                return string.Empty;
            }
        }
    }
}
