﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bag : MonoBehaviour
{
	public int Columns = 8;
	public List<BagSlot> Slots;

	private void Awake()
	{
		foreach (var slot in Slots)
		{
			slot.OnItemDropped += Slot_OnItemDropped;
		}
	}

	private void Slot_OnItemDropped(BagSlot slot, Item item)
	{
		if (ValidateItemSpace(slot, item))
		{
			if (item.InSlot) 
			{
				ToggleItemSlots(item.InSlot, item);
			}

			ToggleItemSlots(slot, item);
			//item.InSlot?.Toggle();
			//slot.Toggle();
			item.PositionBeforeDrag = slot.transform.position;
			item.InSlot = slot;
		}
	}

	private bool ValidateItemSpace(BagSlot slot, Item item)
	{
		var slotIndex = Slots.IndexOf(slot);

		for(int x = 0; x < item.Shape.GetLength(0); x++)
		{
			for (int y = 0; y < item.Shape.GetLength(1); y++)
			{
				if (item.Shape[x, y])
				{
					var slotX = slotIndex % 8;
					var slotY = slotIndex / 8;

					if (!IsValid(slotX + x - item.Pivot.x, slotY + y - item.Pivot.y))
					{
						return false;
					}
				}
			}
		}

		return true;
	}

	private bool IsValid(int x, int y)
	{
		if (x < 0 || y < 0 || x >= Columns || y >= 5)
		{
			return false;
		}

		if (!Slots[x + (Columns * y)].Empty)
		{
			return false;
		}

		return true;
	}

	private void ToggleItemSlots(BagSlot slot, Item item)
	{
		var slotIndex = Slots.IndexOf(slot);

		for (int x = 0; x < item.Shape.GetLength(0); x++)
		{
			for (int y = 0; y < item.Shape.GetLength(1); y++)
			{
				if (item.Shape[x, y])
				{
					var slotX = slotIndex % 8;
					var slotY = slotIndex / 8;

					ToggleSlot(slotX + x - item.Pivot.x, slotY + y - item.Pivot.y);
				}
			}
		}
	}

	private void ToggleSlot(int x, int y)
	{
		Slots[x + (Columns * y)].Toggle();
	}
}