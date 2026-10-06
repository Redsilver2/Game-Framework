using RedSilver2.Framework.Subtitles;
using UnityEngine;

namespace RedSilver2.Framework.References {
    public class ScreenSpaceSubtitleAnchorReference : SubtitleDisplayerAnchorReference
    {
        protected sealed override Quaternion GetDesiredRotation(Transform transform) {
            return Quaternion.identity;
        }
    }
}
