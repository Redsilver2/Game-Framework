using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    [RequireComponent(typeof(Canvas))]
    public sealed class SubtitleParentSetup : MonoBehaviour {
        [SerializeField] private SubtitleParentInfo info;
        [SerializeField] private RectTransform parent;

        private void Awake(){
           Canvas canvas = GetComponent<Canvas>();
           canvas.renderMode = RenderMode.WorldSpace;
        }

        private void OnEnable() { info?.SetParent(parent); }
    }
}
