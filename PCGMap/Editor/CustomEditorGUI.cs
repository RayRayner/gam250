using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIFunctionality
{
    [CustomEditor(typeof(MapGen))]
    public class CustomEditorGUI : Editor
    {

        float MinValue = 0;
        float MaxVaulue = 100;
        float MinLimit = 0;
        float MaxLimit = 200;

        public override void OnInspectorGUI()
        {

            serializedObject.Update();
            MapGen mappy = target as MapGen;
            EditorGUILayout.BeginVertical();

            //Toolbar
            GUIContent[] toolbarOptions = new GUIContent[2];
            toolbarOptions[0] = new GUIContent("PCG");
            toolbarOptions[1] = new GUIContent("Image");
            mappy.toolModeInt = GUILayout.Toolbar(mappy.toolModeInt, toolbarOptions);
            EditorGUILayout.Separator();

            switch (mappy.toolModeInt)
            {

                case 0:

                    //Toolbar
                    GUIContent[] Options = new GUIContent[4];
                    Options[0] = new GUIContent("Variables");
                    Options[1] = new GUIContent("Images");
                    Options[2] = new GUIContent("Debug");
                    Options[3] = new GUIContent("4th Option");
                    mappy.optionsInt = GUILayout.Toolbar(mappy.optionsInt, Options);
                    EditorGUILayout.Separator();

                    switch (mappy.optionsInt)
                    {

                        case 0:
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Max Height");
                            mappy.MaxHeight = EditorGUILayout.Slider(mappy.MaxHeight, 0f, 100f);
                            EditorGUILayout.EndHorizontal();

                            
                            /*
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Scale Min");
                            mappy.ScaleMin = EditorGUILayout.Slider(mappy.ScaleMin, 0, mappy.ScaleMax);
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Scale Max");
                            mappy.ScaleMax = EditorGUILayout.Slider(mappy.ScaleMax, mappy.ScaleMin + 1, 100);
                            EditorGUILayout.EndHorizontal();
                            
                            //DONT KNOW WHICH ONE TO USE
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Scale");
                            mappy.scale = EditorGUILayout.Slider(mappy.scale, 1, 100);
                            EditorGUILayout.EndHorizontal();
                            */

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.MinMaxSlider(ref mappy.ScaleMin, ref mappy.ScaleMax, MinLimit, MaxLimit);
                            EditorGUILayout.EndHorizontal();
                            

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Scale Max = " + Mathf.FloorToInt(mappy.ScaleMax));
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Scale Min = " + Mathf.FloorToInt(mappy.ScaleMin));
                            EditorGUILayout.EndHorizontal();
                           
                            EditorGUILayout.BeginHorizontal();
                          //mappy.scale = Random.Range(mappy.ScaleMin, mappy.ScaleMax);
                            EditorGUILayout.LabelField("Scale = " + Mathf.FloorToInt(mappy.scale));
                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Seed");
                            mappy.seed = EditorGUILayout.IntField(mappy.seed);
                            EditorGUILayout.EndHorizontal();

                            break;

                        case 1:
                            /*
                            Texture2D myTexture = AssetPreview.GetAssetPreview(mappy.splat0);
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Splat 0");
                            GUILayout.Label(myTexture);
                            */

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.ObjectField("Splat 0", mappy.splat0, typeof(Texture2D), false);
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.ObjectField("Splat 1", mappy.splat1, typeof(Texture2D), false);
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.ObjectField("Detail 0", mappy.detail0, typeof(Texture2D), false);
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.ObjectField("Detail 1", mappy.detail1, typeof(Texture2D), false);
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.ObjectField("Detail 2", mappy.detail2, typeof(Texture2D), false);
                            EditorGUILayout.EndHorizontal();

                            break;

                        case 2:


                            break;

                        case 3:

                            EditorGUILayout.BeginHorizontal();
                            if(GUILayout.Button("Save Current Map"))
                            {
                                mappy.SaveTexture();
                            }
                            GUILayout.EndHorizontal();

                            break;

                    }
                    break;

                case 1:

                    

                    break;
                
            }
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

