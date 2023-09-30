using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MatrixSizeAttribute))]
public class MatrixSizeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		int rows = 2;
		int columns = 3;

		bool[,] matrix = new bool[rows, columns];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				SerializedProperty element = property.GetArrayElementAtIndex(i * columns + j);
				matrix[i, j] = element.boolValue;
			}
		}

		EditorGUI.LabelField(position, label);

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				bool newValue = EditorGUI.Toggle(new Rect(position.x + 15 + j * 20, position.y + 20 + i * 20, 15, 15), matrix[i, j]);
				matrix[i, j] = newValue;
				SerializedProperty element = property.GetArrayElementAtIndex(i * columns + j);
				element.boolValue = newValue;
			}
		}

		EditorGUI.EndProperty();
	}
}