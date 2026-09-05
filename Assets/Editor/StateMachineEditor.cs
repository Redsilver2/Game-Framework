using RedSilver2.Framework.StateMachines.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

namespace RedSilver2.Framework.StateMachines.EditorTools
{

    [CustomEditor(typeof(StateMachine))]
    public abstract class StateMachineEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            StateMachine stateMachine = target as StateMachine;
            stateMachine?.DrawInspector();
            EditorUtility.SetDirty(stateMachine);
        }
    }


}
#endif