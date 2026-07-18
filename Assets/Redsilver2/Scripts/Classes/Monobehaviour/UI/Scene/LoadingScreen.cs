using RedSilver2.Framework.Scenes.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public class LoadingScreen : MonoBehaviour
    {
        private List<LoadingScreenUI> loadingOperations;
  
        private void Awake()
        {
            loadingOperations = GetComponentsInChildren<LoadingScreenUI>(true).ToList();
        }

        private void Start()
        {
            SetLoadingOperationsActifState(false);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            SetLoadingOperationsActifState(false);
        }

        private void OnEnable()
        {
            SetLoadingOperationsActifState(true);
        }


        private void SetLoadingOperationsActifState(bool isActif)
        {
            if (loadingOperations != null)
            {
                foreach (LoadingScreenUI operation in loadingOperations) {
                    operation?.CancelFade();
                    operation?.SetAlpha(0f);
                }
            }
        }

        public void Show() {
            StartFade(true);
        }

        public void Hide() {
            StartFade(false);
        }

        private void StartFade(bool isFadeIn) {
            LoadingScreenUI[] operations = GetOperations();
            foreach (LoadingScreenUI operation in operations) operation?.StartFade(isFadeIn);
        }

        public bool IsDoneFading(bool isFadeIn)
        {
            var cleanOperations = GetOperations();
            var results = cleanOperations.Where(x => x.IsTargetedAlpha(isFadeIn ? 1f : 0f)).ToArray();
            return results.Length == cleanOperations.Length;
        }

        private LoadingScreenUI[] GetOperations()
        {
            if (loadingOperations != null) {
               loadingOperations = loadingOperations.Where(x => x != null).ToList();
               return loadingOperations.ToArray();
            }

            return new LoadingScreenUI[0];
        }
    }
}
