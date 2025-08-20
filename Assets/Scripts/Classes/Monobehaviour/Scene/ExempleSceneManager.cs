using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public sealed class ExempleSceneManager : SceneLoaderManager
    {
        private bool flip = false;

        protected void Start()
        {
            AddSceneData(new SceneData[]
            {
                new TestScene01(0, true),
                new TestScene01(1, false)
            });

            Debug.Log(GetAllScenesDatas().Length);
        }
    }

   
}
