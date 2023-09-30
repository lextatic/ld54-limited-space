using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagSlot : MonoBehaviour, IDropHandler
{
	public Image SlotImage;

	public bool Empty;

	public event Action<BagSlot, Item> OnItemDropped;

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
		var item = droppedItem.GetComponent<Item>();
		OnItemDropped.Invoke(this, item);
	}
}
