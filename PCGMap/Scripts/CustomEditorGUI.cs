using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIFunctionality
{
    [CustomEditor(typeof(MapGen))]
    public class CustomEditorGUI : Editor
    {

        public void OnEnable ()
        {

        }

        public override void OnInspectorGUI()
        {

            MapGen mappy = target as MapGen;

            GUI.skin = null;

            
            EditorGUILayout.BeginVertical();
                
            //ToolBar
            GUIContent[] toolbarOptions = new GUIContent[2];
            toolbarOptions[0] = new GUIContent("PCG");
            toolbarOptions[1] = new GUIContent("Image");
            mappy.toolModeInt = GUILayout.Toolbar(mappy.toolModeInt, toolbarOptions);
            EditorGUILayout.Separator();

            //Switch
            switch(mappy.toolModeInt)
        {
            case 0:

                if (GUI.changed) EditorUtility.SetDirty(mappy);
                GUI.changed = false;

                break;
            case 1:
                    break;
        }
            
        }
    }
}