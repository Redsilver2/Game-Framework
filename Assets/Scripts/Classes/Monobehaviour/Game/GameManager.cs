using Redsilver2.Framework.Inputs;
using UnityEngine;

namespace Redsilver2.Framework
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public static GameManager Instance => instance;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;

            Debug.Log(InputManager.GetKeyboardPaths().Length);
            foreach(var result in InputManager.GetGamepadPaths()) Debug.Log(result);
        }
    }
}
