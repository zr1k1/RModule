namespace RModule.Runtime.Arcade.Inventory {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class ItemCellInventoryVC : MonoBehaviour {
		// Outlets
		//[SerializeField] RectTransform _itemContainer = default;
		[SerializeField] Image _itemImage = default;

		// Privats
		InventoryController.InventoryItem _inventoryItem;

		public ItemCellInventoryVC Setup(InventoryController.InventoryItem inventoryItem) {
			_inventoryItem = inventoryItem;
			_itemImage.sprite = _inventoryItem.Item.SpriteRenderer.sprite;
			_itemImage.color = Color.white;

			return this;
		}

		public void Clear() {
			_itemImage.sprite = null;
			_itemImage.color = Color.clear;
			_inventoryItem = null;
		}
	}
}