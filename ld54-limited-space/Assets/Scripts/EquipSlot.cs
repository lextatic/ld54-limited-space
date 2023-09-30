using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IDropHandler
{
	public ItemType ItemType;

	private Image _slotImage;

	public event Action<EquipSlot, Item> OnItemDroppedOnEquipSlot;
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
		OnItemDroppedOnEquipSlot.Invoke(this, item);
	}
}
