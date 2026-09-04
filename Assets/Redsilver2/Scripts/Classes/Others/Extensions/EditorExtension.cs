using UnityEngine;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;

public static class EditorExtension {

    public static void DrawHorizontal(UnityAction onDraw)
    {
        DrawHorizontal(onDraw, false);
    }

    public static void DrawHorizontal(UnityAction onDraw, bool updateIndent)
    {
        EditorGUILayout.BeginHorizontal();
        if(updateIndent) IncrementIndent();

        onDraw?.Invoke();
        EditorGUILayout.EndHorizontal();
        if(updateIndent) DecrementIndent();
    }


    public static void DrawVertical(UnityAction onDraw)
    {
        DrawVertical(onDraw, false);
    }

    public static void DrawVertical(UnityAction onDraw, bool updateIndent)
    {
        EditorGUILayout.BeginVertical();
        if (updateIndent) IncrementIndent();

        onDraw?.Invoke();
        EditorGUILayout.EndVertical();
        if (updateIndent) DecrementIndent();
    }

    public static void DrawHorizontalHelpBox(UnityAction onDraw)
    {
        DrawHorizontalHelpBox(onDraw, Color.black, false);
    }

    public static void DrawHorizontalHelpBox(UnityAction onDraw, bool updateIndent)
    {
        DrawHorizontalHelpBox(onDraw, Color.black, updateIndent);
    }

    public static void DrawHorizontalHelpBox(UnityAction onDraw, Color backgroundColor) {
        DrawHorizontalHelpBox(onDraw, backgroundColor, false);
    }

    public static void DrawHorizontalHelpBox(UnityAction onDraw, Color backgroundColor, bool updateIndent)
    {
        Rect rect = EditorGUILayout.BeginHorizontal("HelpBox");
        if (updateIndent) IncrementIndent();

        if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, backgroundColor);

        onDraw?.Invoke();
        EditorGUILayout.EndHorizontal();
        if (updateIndent) DecrementIndent();
    }

    public static void DrawVerticalHelpBox(UnityAction onDraw)
    {
        DrawVerticalHelpBox(onDraw, Color.black, false);
    }



    public static void DrawVerticalHelpBox(UnityAction onDraw, bool updateIndent)
    {
        DrawVerticalHelpBox(onDraw, Color.black, updateIndent);
    }



    public static void DrawVerticalHelpBox(UnityAction onDraw, Color backgroundColor)
    {
        DrawVerticalHelpBox(onDraw, backgroundColor, false);
    }


    public static void DrawVerticalHelpBox(UnityAction onDraw, Color backgroundColor, bool updateIndent) {

        Rect rect = EditorGUILayout.BeginVertical("HelpBox");
        if (updateIndent)  IncrementIndent();

        if(Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, backgroundColor);

        onDraw?.Invoke();
        EditorGUILayout.EndVertical();
        if (updateIndent) DecrementIndent();
    }



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


    public static void Space(float space) {
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

    public static void DisplayLabel(string label)
    {
        EditorGUILayout.LabelField(label);
    }

    public static void DisplayBoldLabel(string label) {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
    }

    public static void DisplayButton(string label, UnityAction clickAction)
    {
        DisplayButton(label, default, clickAction);
    }


    public static void DisplayButton(string label, Color buttonColor, UnityAction clickAction)
    {
        EditorGUILayout.BeginHorizontal();   
        Color originalColor = GUI.backgroundColor;
      
        GUI.backgroundColor = buttonColor; 
        if (GUILayout.Button(label)) clickAction?.Invoke();
      
        GUI.backgroundColor = originalColor;
        EditorGUILayout.EndHorizontal();
    }

    public static uint DisplayUIntSlider(string label, uint currentValue, uint maxValue)
    {
            return (uint)DisplayIntSlider(label, (int)currentValue, (int)uint.MinValue, (int)maxValue);
    }


        public static int DisplayIntSlider(string label, int currentValue, int minValue, int maxValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            
            currentValue = EditorGUILayout.IntSlider(currentValue, minValue, maxValue);
            EditorGUILayout.EndHorizontal();
            
            return currentValue;
        }

        public static float DisplayFloatSlider(string label, float currentValue, float minValue, float maxValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        
            currentValue = EditorGUILayout.Slider(currentValue, minValue, maxValue);
            EditorGUILayout.EndHorizontal();

            return currentValue;
        }


        public static bool DisplayToggle(string label, bool currentValue)
        {
            EditorGUILayout.BeginHorizontal();
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

    public static bool DisplayBoldFoldout(string label, ref bool currentValue, Color rectColor)
    {
        Rect rect = EditorGUILayout.BeginVertical("HelpBox");
        if (Event.current.type == EventType.Repaint) EditorGUI.DrawRect(rect, rectColor);

        currentValue = EditorGUILayout.Foldout(currentValue, label, true, EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        return currentValue;
    }

    public static Color DisplayColorField(string label, Color currentColor)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        currentColor = EditorGUILayout.ColorField(string.Empty, currentColor);
        EditorGUILayout.EndHorizontal();
        return currentColor;
    }


    public static Vector3 DisplayVector3Field(string label, Vector3 currentValue)
    {
            EditorGUILayout.BeginHorizontal();
    
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            currentValue = EditorGUILayout.Vector3Field(string.Empty, currentValue);

            EditorGUILayout.EndHorizontal();
            return currentValue;
    }

        public static T DisplayCustomField<T>(string label, bool isScenePrefabAllowed, T value) where T : UnityEngine.Object
        {
            EditorGUILayout.BeginHorizontal();
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
