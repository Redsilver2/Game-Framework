using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.UI
{
    public class UISelectionSizeUpdater : UISelectionUpdater {
        [SerializeField] private float sizeUpdateDuration = 0.1f;

        [Space]
        [SerializeField] private Vector3 selectedSize;
        [SerializeField] private Vector3 deselectedSize;

        protected sealed override IEnumerator UpdateUISelection(bool isSelected)
        {
            UISelection selection = Selection;
            yield return StartCoroutine(UpdateSize(selection != null ? selection.transform : null,
                                                   isSelected ? selectedSize : deselectedSize));
        }

        private IEnumerator UpdateSize(Transform transform, Vector3 size)
        {
            Vector3 currentSize = transform != null ? transform.localScale : Vector3.zero;
            float t = 0f;
            
            while(true)
            {
                float progress = Mathf.Clamp01(t/sizeUpdateDuration);

                if (transform != null)
                    transform.localScale = progress < 1f ? Vector3.Lerp(currentSize, size, progress) : size;

                if (progress >= 1f) break;
                t += Time.deltaTime;
                yield return null;
            }         
        }
    }

}
