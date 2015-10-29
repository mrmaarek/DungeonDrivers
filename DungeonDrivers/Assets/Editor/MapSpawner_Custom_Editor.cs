using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapSpawnerScript))] 
public class MapSpawner_Custom_Editor : Editor 
{
	public override void OnInspectorGUI () 
	{
		serializedObject.Update();

		MapSpawnerScript MapSpawnerScript = (MapSpawnerScript)target;
		
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (serializedObject.FindProperty("startSpawning"));

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (serializedObject.FindProperty("currentLevelType"));
		
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (serializedObject.FindProperty("cameraSpeed"));

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (serializedObject.FindProperty("tiles"), true);

		GUIStyle s = new GUIStyle(EditorStyles.label);

		if(MapSpawnerScript.totalPercentage < 100 || MapSpawnerScript.totalPercentage > 100)
		{
			s.normal.textColor = Color.red;
		}
		else
		{
			s.normal.textColor = Color.black;
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField ("Currently the total percentage is " + MapSpawnerScript.totalPercentage + "% this should be 100% at all times.", s);

		serializedObject.ApplyModifiedProperties();
		
	}
}

[CustomPropertyDrawer (typeof (TileForList))]
public class TileForSpawning_Custom_Editor : PropertyDrawer 
{
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) 
	{

		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty Tile = prop.FindPropertyRelative ("Tile");
		SerializedProperty spawnPercentage = prop.FindPropertyRelative ("spawnPercentage");

		EditorGUI.PropertyField(new Rect(pos.x, pos.y, 200, pos.height), name, GUIContent.none);
		EditorGUI.PropertyField(new Rect(pos.x + 200, pos.y, 120, pos.height), Tile, GUIContent.none);


		if(GUI.Button(new Rect(pos.x + 330, pos.y, 40, pos.height), "- 5"))
		{
			spawnPercentage.intValue = newPercentage(-5, spawnPercentage.intValue);
		}
		
		if(GUI.Button(new Rect(pos.x + 370, pos.y, 40, pos.height), "- 1"))
		{
			spawnPercentage.intValue = newPercentage(-1, spawnPercentage.intValue);
		}

		EditorGUI.LabelField(new Rect(pos.x + 405, pos.y, 60, pos.height), spawnPercentage.intValue + "%");
		
		if(GUI.Button(new Rect(pos.x + 460, pos.y, 40, pos.height), "+ 1"))
		{
			spawnPercentage.intValue = newPercentage(1, spawnPercentage.intValue);
		}
		
		if(GUI.Button(new Rect(pos.x + 500, pos.y, 40, pos.height), "+ 5"))
		{
			spawnPercentage.intValue = newPercentage(5, spawnPercentage.intValue);
		}
	}
	public int newPercentage(int changeNumber, int oldNumber)
	{
		if(changeNumber == -5 && oldNumber > 4)
		{
			return oldNumber -= 5;
		}
		else if(changeNumber == -1 && oldNumber > 0)
		{
			return oldNumber -= 1;
		}
		else if(changeNumber == 1 && oldNumber < 100)
		{
			return oldNumber += 1;
		}
		else if(changeNumber == 5 && oldNumber < 96)
		{
			return oldNumber += 5;
		}
		else
		{
			return oldNumber;
		}
	}
}

