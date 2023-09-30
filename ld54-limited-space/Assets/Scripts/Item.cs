using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private const int Columns = 3;
	private const int Rows = 2;
	private const float CellSize = 65;

	public int Value;

	public ItemType Type;

	public bool[,] Shape = new bool[Rows, Columns] { { true, true, true }, { true, false, false } };
	public bool[,] UnrotatedShape;

	public Vector2Int Pivot;
	public Vector2Int UnrotatedPivot;

	public Image ItemImage;
	public Quaternion UnrotatedImageRotation;

	private bool _dragging;
	private bool _movedToNewSlot;

	public Vector2 PositionBeforeDrag;

	public BagSlot InSlot;

	private void Awake()
	{
		ItemImage = GetComponent<Image>();

		ItemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CellSize * Shape.GetLength(1));
		ItemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CellSize * Shape.GetLength(0));

		ItemImage.rectTransform.pivot = new Vector2(
			((CellSize / 2) + (CellSize * Pivot.x)) / ItemImage.rectTransform.rect.width,
			1 - (((CellSize / 2) + (CellSize * Pivot.y)) / ItemImage.rectTransform.rect.height));

		_dragging = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_dragging = true;
		_movedToNewSlot = false;
		PositionBeforeDrag = transform.position;
		ItemImage.raycastTarget = false;

		UnrotatedShape = Shape;
		UnrotatedPivot = Pivot;
		UnrotatedImageRotation = ItemImage.rectTransform.rotation;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_dragging = false;

		if (!_movedToNewSlot)
		{
			Shape = UnrotatedShape;
			Pivot = UnrotatedPivot;
			ItemImage.rectTransform.rotation = UnrotatedImageRotation;

		}

		transform.position = PositionBeforeDrag;
		ItemImage.raycastTarget = true;
	}

	public void MoveToSlot(BagSlot newSlot)
	{
		PositionBeforeDrag = newSlot.transform.position;
		InSlot = newSlot;
		_movedToNewSlot = true;
	}

	private void RotateCounterclockwise()
	{
		ItemImage.rectTransform.Rotate(0, 0, 90, Space.Self);

		var rows = Shape.GetLength(0);
		var columns = Shape.GetLength(1);

		bool[,] rotatedMatrix = new bool[columns, rows];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				rotatedMatrix[columns - 1 - j, i] = Shape[i, j];
			}
		}

		Shape = rotatedMatrix;

		int rotatedX = Pivot.y;
		int rotatedY = columns - 1 - Pivot.x;

		Pivot = new Vector2Int(rotatedX, rotatedY);
	}

	private void RotateClockwise()
	{
		ItemImage.rectTransform.Rotate(0, 0, -90, Space.Self);

		var rows = Shape.GetLength(0);
		var columns = Shape.GetLength(1);

		bool[,] rotatedMatrix = new bool[columns, rows];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				rotatedMatrix[j, rows - 1 - i] = Shape[i, j];
			}
		}

		Shape = rotatedMatrix;

		int rotatedX = rows - 1 - Pivot.y;
		int rotatedY = Pivot.x;

		Pivot = new Vector2Int(rotatedX, rotatedY);
	}

	private void Update()
	{
		if (_dragging)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				RotateCounterclockwise();
			}

			if (Input.GetKeyDown(KeyCode.E))
			{
				RotateClockwise();
			}
		}
	}
}
