using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
	public int Columns = 8;

	public List<BagSlot> BagSlots;
	public List<EquipSlot> EquipSlots;
	public ItemSwapArea SwapArea;

	private void Awake()
	{
		foreach (var bagSlot in BagSlots)
		{
			bagSlot.OnItemDroppedOnBagSlot += Slot_OnItemDropped;
		}

		foreach (var equipSlot in EquipSlots)
		{
			equipSlot.OnItemDroppedOnEquipSlot += EquipSlot_OnItemDroppedOnEquipSlot;
		}

		SwapArea.OnItemDroppedOnItemSwapArea += SwapArea_OnItemDroppedOnItemSwapArea;
	}

	private void EquipSlot_OnItemDroppedOnEquipSlot(EquipSlot equipSlot, Item item)
	{
		if (ValidateEquipType(equipSlot, item))
		{
			RemoveFromPreviousSlots(item);

			equipSlot.Toggle();

			item.MoveToEquipSlot(equipSlot);
		}
	}

	private void SwapArea_OnItemDroppedOnItemSwapArea(Item item)
	{
		RemoveFromPreviousSlots(item);

		item.MoveToSwapArea();
	}

	private void Slot_OnItemDropped(BagSlot bagSlot, Item item)
	{
		if (ValidateItemSpace(bagSlot, item))
		{
			RemoveFromPreviousSlots(item);

			ToggleItemSlots(bagSlot, item);

			item.MoveToBagSlot(bagSlot);
		}
	}

	private void RemoveFromPreviousSlots(Item item)
	{
		if (item.CurrentBagSlot)
		{
			ToggleItemSlots(item.CurrentBagSlot, item, true);
		}

		item.CurrentEquipSlot?.Toggle();
	}

	private bool ValidateItemSpace(BagSlot bagSlot, Item item)
	{
		var bagSlots = new bool[BagSlots.Count];

		for (int i = 0; i < BagSlots.Count; i++)
		{
			bagSlots[i] = BagSlots[i].Empty;
		}

		if (item.CurrentBagSlot)
		{
			ToggleMatyricSlots(item.CurrentBagSlot, item, bagSlots, true);
		}

		var slotIndex = BagSlots.IndexOf(bagSlot);

		for (int x = 0; x < item.ShapeMatrix.GetLength(1); x++)
		{
			for (int y = 0; y < item.ShapeMatrix.GetLength(0); y++)
			{
				if (item.ShapeMatrix[y, x])
				{
					var slotX = slotIndex % 8;
					var slotY = slotIndex / 8;

					if (!IsValid(slotX + x - item.Pivot.x, slotY + y - item.Pivot.y, bagSlots))
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

		var slotIndex = BagSlots.IndexOf(slot);

		for (int x = 0; x < shape.GetLength(1); x++)
		{
			for (int y = 0; y < shape.GetLength(0); y++)
			{
				if (shape[y, x])
				{
					var slotX = slotIndex % 8;
					var slotY = slotIndex / 8;

					ToggleBagSlot(slotX + x - pivot.x, slotY + y - pivot.y);
				}
			}
		}
	}

	private void ToggleMatyricSlots(BagSlot bagSlot, Item item, bool[] bagSlots, bool unrotated = false)
	{
		var shape = unrotated ? item.UnrotatedShape : item.ShapeMatrix;
		var pivot = unrotated ? item.UnrotatedPivot : item.Pivot;

		var slotIndex = BagSlots.IndexOf(bagSlot);

		for (int x = 0; x < shape.GetLength(1); x++)
		{
			for (int y = 0; y < shape.GetLength(0); y++)
			{
				if (shape[y, x])
				{
					var slotX = slotIndex % 8;
					var slotY = slotIndex / 8;

					bagSlots[slotX + x - pivot.x + (Columns * (slotY + y - pivot.y))] = !bagSlots[slotX + x - pivot.x + (Columns * (slotY + y - pivot.y))];
				}
			}
		}
	}

	private void ToggleBagSlot(int x, int y)
	{
		BagSlots[x + (Columns * y)].Toggle();
	}

	private bool ValidateEquipType(EquipSlot equipSlot, Item item)
	{
		return item.Type == equipSlot.ItemType;
	}
}