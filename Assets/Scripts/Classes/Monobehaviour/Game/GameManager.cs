using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Scenes;
using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}
