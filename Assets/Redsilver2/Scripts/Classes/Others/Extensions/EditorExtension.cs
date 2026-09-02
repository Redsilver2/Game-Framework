using UnityEngine;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;

    public static class EditorExtension {

        public static void IncrementIndent()
        {
            EditorGUI.indentLevel++;
        }

        public static void IncrementIndent(uint value)
        {
            EditorGUI.indentLevel += (int)value;
        }

        public static void DecrementIndent()
        {
            EditorGUI.indentLevel--;
        }

        public static void DecrementIndent(uint value) {    
            EditorGUI.indentLevel -= (int)value;    
        }


        public static void Space()
        {
            EditorGUILayout.Space();
        }


        public static void Space(float space)  {
            EditorGUILayout.Space(space);
        }

        public static void DisplayHelpBoxNone(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.None);
        }

        public static void DisplayHelpBoxInfo(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Info);
        }

        public static void DisplayHelpBoxWarning(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }

        public static void DisplayHelpBoxError(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Error);
        }

        public static void DisplayButton(string label, UnityAction clickAction)
        {
            if (GUILayout.Button(label))
                clickAction?.Invoke();
        }

        public static uint DisplayUIntSlider(string label, uint currentValue, uint maxValue, Color rectColor)
        {
            return (uint)DisplayIntSlider(label, (int)currentValue, (int)uint.MinValue, (int)maxValue, rectColor);
        }


        public static int DisplayIntSlider(string label, int currentValue, int minValue, int maxValue, Color rectColor)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, rectColor);

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            int newValue = EditorGUILayout.IntSlider(currentValue, minValue, maxValue);

            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static float DisplayFloatSlider(string label, float currentValue, float minValue, float maxValue, Color rectColor)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, rectColor);

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            float newValue = EditorGUILayout.Slider(currentValue, minValue, maxValue);

            EditorGUILayout.EndHorizontal();
            return newValue;
        }


        public static bool DisplayToggle(string label, bool currentValue, Color rectColor)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, rectColor);

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            currentValue = EditorGUILayout.Toggle(string.Empty, currentValue);

            EditorGUILayout.EndHorizontal();
            return currentValue;
        }

        public static bool DisplayFoldout(string label, ref bool currentValue, Color rectColor)
        {
            Rect rect = EditorGUILayout.BeginVertical("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, rectColor);

            currentValue = EditorGUILayout.Foldout(currentValue, label, true);
            EditorGUILayout.EndVertical();
            return currentValue;
        }

        public static Color DisplayColorField(string label, Color currentColor, Color rectColor)
        {
           Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
           if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, rectColor);

           EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
           currentColor = EditorGUILayout.ColorField(string.Empty, currentColor);

           EditorGUILayout.EndHorizontal();
           return currentColor;
        }


        public static Vector3 DisplayVector3Field(string label, Vector3 currentValue, Color rectColor)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, rectColor);

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            currentValue = EditorGUILayout.Vector3Field(string.Empty, currentValue);

            EditorGUILayout.EndHorizontal();
            return currentValue;
        }

        public static T DisplayCustomField<T>(string label, bool isScenePrefabAllowed, T value, Color rectColor) where T : UnityEngine.Object
        {
            Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
            if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, rectColor);

            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            T result = (T)EditorGUILayout.ObjectField(
              value,
              typeof(T),
              isScenePrefabAllowed
            );

            EditorGUILayout.EndHorizontal();
            return result;
        }
    }

#endif
