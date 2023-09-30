using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[Header("Bag visual config")]
	public float CellSize = 65;

	[Header("Grid Config")]
	public int Columns = 3;
	public int Rows = 2;
	public Vector2Int Pivot;
	public bool[] Shape;

	[Header("Gameplay")]
	public int Value;
	public ItemType Type;

	[HideInInspector]
	public BagSlot InSlot;

	private Image _itemImage;
	private bool _dragging;
	private bool _movedToNewPosition;
	private Vector2 _positionBeforeDrag;

	public bool[,] ShapeMatrix { get; private set; }
	public bool[,] UnrotatedShape { get; private set; }
	public Quaternion UnrotatedImageRotation { get; private set; }
	public Vector2Int UnrotatedPivot { get; private set; }

	private void Awake()
	{
		InitializeShapeMatrix();
		InitializeImageSizeAndPivot();

		_dragging = false;

		_itemImage.alphaHitTestMinimumThreshold = 1f;
	}

	private void InitializeShapeMatrix()
	{
		ShapeMatrix = new bool[Rows, Columns];
		int count = 0;
		for (int x = 0; x < Rows; x++)
		{
			for (int y = 0; y < Columns; y++)
			{
				ShapeMatrix[x, y] = Shape[count];
				count++;
			}
		}
	}

	private void InitializeImageSizeAndPivot()
	{
		_itemImage = GetComponent<Image>();

		_itemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CellSize * ShapeMatrix.GetLength(1));
		_itemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CellSize * ShapeMatrix.GetLength(0));

		_itemImage.rectTransform.pivot = new Vector2(
			((CellSize / 2) + (CellSize * Pivot.x)) / _itemImage.rectTransform.rect.width,
			1 - (((CellSize / 2) + (CellSize * Pivot.y)) / _itemImage.rectTransform.rect.height));
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_dragging = true;
		_movedToNewPosition = false;
		_positionBeforeDrag = transform.position;
		_itemImage.raycastTarget = false;

		UnrotatedShape = ShapeMatrix;
		UnrotatedPivot = Pivot;
		UnrotatedImageRotation = _itemImage.rectTransform.rotation;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_dragging = false;

		if (!_movedToNewPosition)
		{
			ShapeMatrix = UnrotatedShape;
			Pivot = UnrotatedPivot;
			_itemImage.rectTransform.rotation = UnrotatedImageRotation;

		}

		transform.position = _positionBeforeDrag;
		_itemImage.raycastTarget = true;
	}

	public void MoveToSlot(BagSlot newSlot)
	{
		_positionBeforeDrag = newSlot.transform.position;
		InSlot = newSlot;
		_movedToNewPosition = true;
	}

	public void MoveToSwapArea()
	{
		_positionBeforeDrag = Input.mousePosition;
		InSlot = null;
		_movedToNewPosition = true;
	}

	private void RotateCounterclockwise()
	{
		_itemImage.rectTransform.Rotate(0, 0, 90, Space.Self);

		var rows = ShapeMatrix.GetLength(0);
		var columns = ShapeMatrix.GetLength(1);

		bool[,] rotatedMatrix = new bool[columns, rows];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				rotatedMatrix[columns - 1 - j, i] = ShapeMatrix[i, j];
			}
		}

		ShapeMatrix = rotatedMatrix;

		int rotatedX = Pivot.y;
		int rotatedY = columns - 1 - Pivot.x;

		Pivot = new Vector2Int(rotatedX, rotatedY);
	}

	private void RotateClockwise()
	{
		_itemImage.rectTransform.Rotate(0, 0, -90, Space.Self);

		var rows = ShapeMatrix.GetLength(0);
		var columns = ShapeMatrix.GetLength(1);

		bool[,] rotatedMatrix = new bool[columns, rows];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				rotatedMatrix[j, rows - 1 - i] = ShapeMatrix[i, j];
			}
		}

		ShapeMatrix = rotatedMatrix;

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

	public void ToggleRaycastTarget(bool value)
	{
		_itemImage.raycastTarget = value;
	}
}
