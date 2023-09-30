//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;

//[CustomEditor(typeof(Item))]
//public class BoolMatrixDrawer : Editor
//{
//	private ReorderableList reorderableList;

//	private void OnEnable()
//	{
//		SerializedProperty boolMatrix = serializedObject.FindProperty("Shape");
//		reorderableList = new ReorderableList(serializedObject, boolMatrix)
//		{
//			drawHeaderCallback = rect => EditorGUI.LabelField(rect, boolMatrix.displayName),
//			drawElementCallback = (rect, index, isActive, isFocused) =>
//			{
//				var element = boolMatrix.GetArrayElementAtIndex(index);
//				EditorGUI.PropertyField(rect, element, GUIContent.none);
//			},
//			elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
//		};
//	}

//	public override void OnInspectorGUI()
//	{
//		serializedObject.Update();
//		reorderableList.DoLayoutList();
//		serializedObject.ApplyModifiedProperties();
//	}
//}