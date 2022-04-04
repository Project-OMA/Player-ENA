using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AccessibleInteractable<>), true), CanEditMultipleObjects]
public class Accessibility_Interactable_Inspector : Accessibility_InspectorShared
{
	//////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{
	}

	//////////////////////////////////////////////////////////////////////////

	public override void OnInspectorGUI()
	{
		SetupGUIStyles();
		serializedObject.Update();

		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("FileBrowserItem", myHeadingStyle);
		EditorGUILayout.Separator();

		// Name
		DrawNameSection();

		// Reference / Target 
		DrawTargetSection("FileBrowserItem");

		// Positioning / Traversal
		DrawPositionOrderSection();

		// Speech Output
		DrawSpeechOutputSection();

		// Callbacks
		DrawCallbackSection(true);


		serializedObject.ApplyModifiedProperties();
		DrawDefaultInspectorSection();
	}

}
