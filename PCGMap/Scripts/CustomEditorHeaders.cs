using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CustomEditorHeaders : PropertyAttribute
{
    //Variables
    public string HeaderText;
    public float Spacing;
    public Color colour;

    public CustomEditorHeaders(string HeaderText, float Spacing, float r, float g, float b, float a)
    {
        //Setting variables
        this.HeaderText = HeaderText;
        this.Spacing = Spacing;
        this.colour = new Color(r/255, g/255, b/255, a);
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CustomEditorHeaders))]
public class CustomScriptDrawer : DecoratorDrawer
{
	CustomEditorHeaders customFunctions
	{
		get { return ((CustomEditorHeaders)attribute); }
	}

	public override float GetHeight ()
	{
		
		return base.GetHeight () + customFunctions.Spacing;
	}

	public override void OnGUI (Rect position)
	{
		//Making the style
		GUIStyle style = new GUIStyle ();
		//Bold text
		style.fontStyle = FontStyle.Bold;
		//Alignment
		style.alignment = TextAnchor.MiddleLeft;
		//Colour
		style.normal.textColor = customFunctions.colour;
		//Position
		float lineX = (position.x);
		float lineY = (position.y + (position.height/10));
		//The visual part
		EditorGUI.LabelField (new Rect (lineX, lineY, position.width, 20), customFunctions.HeaderText, style);
	}
}
#endif