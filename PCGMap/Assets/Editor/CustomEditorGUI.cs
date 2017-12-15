using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIFunctionality
{
    [CustomEditor(typeof(MapGen))]
    public class CustomEditorGUI : Editor
    {
        float MinLimit = 0;
        float MaxLimit = 200;
        MapGen mappy;

        const int VARIABLES_TAB = 0;
        const int TEXTURES_TAB = 1;
        const int DEBUG_TAB = 2;
        const int FOURTH_OPTION = 3;

        const int PICTURE_VARIABLES_TAB = 0;
        const int PICTURE_TEXTURES_TAB = 1;
        const int PICTURE_PREDEFINED_MAP = 2;

        public void DisplayVariables()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Max Height");
            mappy.MaxHeight = EditorGUILayout.Slider(mappy.MaxHeight, 0f, 100f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scale is Random between " + Mathf.FloorToInt(mappy.ScaleMin) + " and " + Mathf.FloorToInt(mappy.ScaleMax) + " (Roughly)");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.MinMaxSlider(ref mappy.ScaleMin, ref mappy.ScaleMax, MinLimit, MaxLimit);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Seed", "If this is Zero, seed will be random"));
            mappy.seed = EditorGUILayout.IntField(mappy.seed);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Octaves");
            mappy.octaves = EditorGUILayout.IntSlider(mappy.octaves, 0, 100);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Lacunarity");
            mappy.lacunarity = EditorGUILayout.FloatField(mappy.lacunarity);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Persistance");
            mappy.persistance = EditorGUILayout.FloatField(mappy.persistance);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Amount of tiles (X Axis)");
            mappy.tileX = EditorGUILayout.IntField(mappy.tileX);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Amount of tiles (Z Axis)");
            mappy.tileZ = EditorGUILayout.IntField(mappy.tileZ);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Terrain Size", "The size of each individual terrain piece"));
            mappy.terrainSize = EditorGUILayout.IntSlider(mappy.terrainSize, 0, 513);
            EditorGUILayout.EndHorizontal();


            /*
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Temp");
            EditorGUILayout.EndHorizontal();
            */
        }

        public void DisplayTextures ()
        {
            EditorGUILayout.BeginHorizontal();
            mappy.splat0 = (Texture2D)EditorGUILayout.ObjectField("Splat 0", mappy.splat0, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            mappy.splat1 = (Texture2D)EditorGUILayout.ObjectField("Splat 1", mappy.splat1, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            mappy.detail0 = (Texture2D)EditorGUILayout.ObjectField("Detail 0", mappy.detail0, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            mappy.detail1 = (Texture2D)EditorGUILayout.ObjectField("Detail 1", mappy.detail1, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            mappy.detail2 = (Texture2D)EditorGUILayout.ObjectField("Detail 2", mappy.detail2, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

        }

        public void PictureDisplayVariables()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Max Height");
            mappy.MaxHeight = EditorGUILayout.Slider(mappy.MaxHeight, 0f, 100f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Amount of tiles (X Axis)");
            mappy.tileX = EditorGUILayout.IntField(mappy.tileX);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Amount of tiles (Z Axis)");
            mappy.tileZ = EditorGUILayout.IntField(mappy.tileZ);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Terrain Size", "The size of each individual terrain piece"));
            mappy.terrainSize = EditorGUILayout.IntSlider(mappy.terrainSize, 0, 512);
            EditorGUILayout.EndHorizontal();
        }

        public void PictureDisplayTextures ()
        {
            EditorGUILayout.BeginHorizontal();
            mappy.splat0 = (Texture2D)EditorGUILayout.ObjectField("Splat 0", mappy.splat0, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            mappy.splat1 = (Texture2D)EditorGUILayout.ObjectField("Splat 1", mappy.splat1, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            mappy.detail0 = (Texture2D)EditorGUILayout.ObjectField("Detail 0", mappy.detail0, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            mappy.detail1 = (Texture2D)EditorGUILayout.ObjectField("Detail 1", mappy.detail1, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            mappy.detail2 = (Texture2D)EditorGUILayout.ObjectField("Detail 2", mappy.detail2, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();
            mappy = target as MapGen;
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
                    Options[1] = new GUIContent("Textures");
                    Options[2] = new GUIContent("Debug");
                    Options[3] = new GUIContent("4th Option");
                    mappy.optionsInt = GUILayout.Toolbar(mappy.optionsInt, Options);
                    EditorGUILayout.Separator();

                    switch (mappy.optionsInt)
                    {

                        case VARIABLES_TAB:
                            DisplayVariables();
                            break;

                        case TEXTURES_TAB:
                            DisplayTextures();                            
                            break;

                        case DEBUG_TAB:

                            break;

                        case FOURTH_OPTION:

                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("Save Current Map"))
                            {
                                mappy.SaveTexture();
                            }
                            GUILayout.EndHorizontal();

                            break;

                    }
                    break;

                case 1:

                    //Toolbar
                    GUIContent[] PreGen = new GUIContent[3];
                    PreGen[0] = new GUIContent("Variables");
                    PreGen[1] = new GUIContent("Textures");
                    PreGen[2] = new GUIContent("Predefined Map");
                    mappy.pictureInt = GUILayout.Toolbar(mappy.pictureInt, PreGen);
                    EditorGUILayout.Separator();

                    switch (mappy.pictureInt)
                    {
                        case PICTURE_VARIABLES_TAB:
                            PictureDisplayVariables();
                            break;

                        case PICTURE_TEXTURES_TAB:
                            PictureDisplayTextures();
                            break;

                        case PICTURE_PREDEFINED_MAP:
                            EditorGUILayout.BeginHorizontal();
                            mappy.PredefinedMap = (Texture2D)EditorGUILayout.ObjectField("Map To Use", mappy.PredefinedMap, typeof(Texture2D), false);
                            EditorGUILayout.EndHorizontal();
                            break;

                    }
                    break;
            }
                
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

