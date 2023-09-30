using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
	public int Columns = 8;

	public List<BagSlot> Slots;
	public ItemSwapArea SwapArea;

	private void Awake()
	{
		foreach (var slot in Slots)
		{
			slot.OnItemDroppedOnBagSlot += Slot_OnItemDropped;
		}

		SwapArea.OnItemDroppedOnItemSwapArea += SwapArea_OnItemDroppedOnItemSwapArea;
	}

	private void SwapArea_OnItemDroppedOnItemSwapArea(Item item)
	{
		if (item.InSlot)
		{
			ToggleItemSlots(item.InSlot, item, true);
		}

		item.MoveToSwapArea();
	}

	private void Slot_OnItemDropped(BagSlot slot, Item item)
	{
		if (ValidateItemSpace(slot, item))
		{
			if (item.InSlot)
			{
				ToggleItemSlots(item.InSlot, item, true);
			}

			ToggleItemSlots(slot, item);

			item.MoveToSlot(slot);
		}
	}

	private bool ValidateItemSpace(BagSlot slot, Item item)
	{
		var slots = new bool[Slots.Count];

		for (int i = 0; i < Slots.Count; i++)
		{
			slots[i] = Slots[i].Empty;
		}

		if (item.InSlot)
		{
			ToggleMatyricSlots(item.InSlot, item, slots, true);
		}

		var slotIndex = Slots.IndexOf(slot);

		for (int x = 0; x < item.ShapeMatrix.GetLength(1); x++)
		{
			for (int y = 0; y < item.ShapeMatrix.GetLength(0); y++)
			{
				if (item.ShapeMatrix[y, x])
				{
					var slotX = slotIndex % 8;
					var slotY = slotIndex / 8;

					if (!IsValid(slotX + x - item.Pivot.x, slotY + y - item.Pivot.y, slots))
					{
						return false;
					}
				}
			}
		}

		return true;
	}

	private bool IsValid(int x, int y, bool[] slots)
	{
		if (x < 0 || y < 0 || x >= Columns || y >= 5)
		{
			return false;
		}

		if (!slots[x + (Columns * y)])
		{
			return false;
		}

		return true;
	}

	private void ToggleItemSlots(BagSlot slot, Item item, bool unrotated = false)
	{
		var shape = unrotated ? item.UnrotatedShape : item.ShapeMatrix;
		var pivot = unrotated ? item.UnrotatedPivot : item.Pivot;

		var slotIndex = Slots.IndexOf(slot);

		for (int x = 0; x < shape.GetLength(1); x++)
		{
			for (int y = 0; y < shape.GetLength(0); y++)
			{
				if (shape[y, x])
				{
					var slotX = slotIndex % 8;
					var slotY = slotIndex / 8;

					ToggleSlot(slotX + x - pivot.x, slotY + y - pivot.y);
				}
			}
		}
	}

	private void ToggleMatyricSlots(BagSlot slot, Item item, bool[] slots, bool unrotated = false)
	{
		var shape = unrotated ? item.UnrotatedShape : item.ShapeMatrix;
		var pivot = unrotated ? item.UnrotatedPivot : item.Pivot;

		var slotIndex = Slots.IndexOf(slot);

		for (int x = 0; x < shape.GetLength(1); x++)
		{
			for (int y = 0; y < shape.GetLength(0); y++)
			{
				if (shape[y, x])
				{
					var slotX = slotIndex % 8;
					var slotY = slotIndex / 8;

					slots[slotX + x - pivot.x + (Columns * (slotY + y - pivot.y))] = !slots[slotX + x - pivot.x + (Columns * (slotY + y - pivot.y))];
				}
			}
		}
	}

	private void ToggleSlot(int x, int y)
	{
		Slots[x + (Columns * y)].Toggle();
	}
}