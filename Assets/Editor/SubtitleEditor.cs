using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace RedSilver2.Framework.Subtitles.Editors {
    [CustomEditor(typeof(Subtitle))]
    public class SubtitleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Subtitle subtitle = target as Subtitle;
            subtitle?.DrawInspector();
            EditorUtility.SetDirty(subtitle);
        }
    }
}
#endif
