using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public int Value;

	public ItemType Type;

	public bool[,] Shape;

	public Vector2 Pivot;

	public Image ItemImage;

	private bool _dragging;

	public Vector2 PositionBeforeDrag;

	private void Awake()
	{
		ItemImage = GetComponent<Image>();

		//int widthSlots = (int)(ItemImage.rectTransform.rect.width / 65);
		//int heightSlots = (int)(ItemImage.rectTransform.rect.height / 65);

		ItemImage.rectTransform.pivot = new Vector2(
			((65f / 2) + (65f * Pivot.x)) / ItemImage.rectTransform.rect.width,
			((65f / 2) + (65f * Pivot.y)) / ItemImage.rectTransform.rect.height);

		_dragging = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("Begin Drag");
		_dragging = true;
		PositionBeforeDrag = transform.position;
		ItemImage.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Debug.Log("Drag");

		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("End Drag");
		_dragging = false;
		transform.position = PositionBeforeDrag;
		ItemImage.raycastTarget = true;
	}

	private void RotateLeft()
	{
		ItemImage.rectTransform.Rotate(0, 0, -90, Space.Self);
	}

	private void Update()
	{
		if (_dragging)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				Debug.Log("Rotate Left");
				RotateLeft();
			}

			if (Input.GetKeyDown(KeyCode.E))
			{
				Debug.Log("Rotate Right");
			}
		}
	}
}
