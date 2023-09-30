using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSwapArea : MonoBehaviour, IDropHandler
{
	public event Action<Item> OnItemDroppedOnItemSwapArea;

	public void OnDrop(PointerEventData eventData)
	{
		var droppedItem = eventData.pointerDrag;
		var item = droppedItem.GetComponent<Item>();
		OnItemDroppedOnItemSwapArea.Invoke(item);
	}
}
