using UnityEngine;

public class MatrixSizeAttribute : PropertyAttribute
{
	public string RowsPropertyName;
	public string ColumnsPropertyName;

	public MatrixSizeAttribute(string rowsProperty, string columnsProperty)
	{
		RowsPropertyName = rowsProperty;
		ColumnsPropertyName = columnsProperty;
	}
}