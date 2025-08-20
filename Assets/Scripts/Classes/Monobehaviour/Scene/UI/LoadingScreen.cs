using RedSilver2.Framework.Scenes.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public class LoadingScreen : MonoBehaviour
    {
        private List<LoadingFade> loadingFades;
  
        private void Awake()
        {
            loadingFades = GetComponentsInChildren<LoadingFade>(true).ToList();
            gameObject.SetActive(false);
        }

        public void CancelAllFades()
        {
            foreach (LoadingFade fade in GetCleanArray()) { fade.CancelFade(); }
        }

        private void SetAllAlpha(bool isVisible)
        {
            foreach (LoadingFade fade in GetCleanArray()) { fade.SetAlpha(isVisible); }
        }

        public bool IsUIVisibilitySet()
        {
            if(loadingFades == null || loadingFades.Count == 0) return false;
            return loadingFades.Where(x => x.IsFadeCompleted).Count() == loadingFades.Count;
        }

        public void Show()
        {
            Fade(true);
        }

        public void Hide()
        {
            Fade(false);
        }

        private void Fade(bool isFadeIn)
        {
            foreach (LoadingFade fade in GetCleanArray()) { fade.StartFade(isFadeIn); }
        }


        private LoadingFade[] GetCleanArray()
        {
            if (loadingFades != null)
            {
               loadingFades = loadingFades.Where(x => x != null).ToList();
               return loadingFades.ToArray();
            }

            return new LoadingFade[0];
        }
    }
}
