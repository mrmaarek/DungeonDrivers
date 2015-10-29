using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer (typeof (Phase))]
public class PhaseWalker_Custom_Editor : PropertyDrawer 
{
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) 
	{
		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty type = prop.FindPropertyRelative ("phaseType");
		SerializedProperty delay = prop.FindPropertyRelative ("doneDelay");
		
		EditorGUI.PropertyField(new Rect (pos.x, pos.y, 160, pos.height), name, GUIContent.none);
		EditorGUI.PropertyField(new Rect (pos.x + 200, pos.y, 120, pos.height), type, GUIContent.none);
		EditorGUI.PropertyField(new Rect (pos.x + 330, pos.y, 50, pos.height), delay, GUIContent.none);



	}
}
