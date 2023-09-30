using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagSlot : MonoBehaviour, IDropHandler
{
	private Image _slotImage;

	public event Action<BagSlot, Item> OnItemDroppedOnBagSlot;
	public bool Empty { get; private set; }

	private void Awake()
	{
		_slotImage = GetComponent<Image>();
		Empty = true;
	}

	public void Toggle()
	{
		Empty = !Empty;

		if (Empty)
		{
			_slotImage.color = Color.white;
		}
		else
		{
			_slotImage.color = Color.gray;
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		var droppedItem = eventData.pointerDrag;
		var item = droppedItem.GetComponent<Item>();
		OnItemDroppedOnBagSlot.Invoke(this, item);
	}
}
