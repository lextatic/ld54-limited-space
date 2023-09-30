using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagSlot : MonoBehaviour, IDropHandler
{
	public Image SlotImage;

	public bool Empty;

	private void Awake()
	{
		SlotImage = GetComponent<Image>();
		Empty = true;
	}

	public void Toggle()
	{
		Empty = !Empty;

		if (Empty)
		{
			SlotImage.color = Color.white;
		}
		else
		{
			SlotImage.color = Color.gray;
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		var droppedItem = eventData.pointerDrag;
		if (droppedItem != null)
		{
			var item = droppedItem.GetComponent<Item>();
			item.PositionBeforeDrag = transform.position;
		}
	}
}
