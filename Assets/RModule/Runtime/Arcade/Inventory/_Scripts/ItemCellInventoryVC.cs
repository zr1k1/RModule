namespace RModule.Runtime.Arcade.Inventory {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using TMPro;

	public class ItemCellInventoryVC : MonoBehaviour {
		// Outlets
		[SerializeField] Image _itemImage = default;
		[SerializeField] TextMeshProUGUI _amount = default;
		[SerializeField] float _changeImageSizeTo = default;

		// Privats
		InventoryController.InventoryItem _inventoryItem;

		public ItemCellInventoryVC Setup(InventoryController.InventoryItem inventoryItem) {
			_inventoryItem = inventoryItem;
			_itemImage.sprite = _inventoryItem.Item.SpriteRenderer.sprite;
			_itemImage.ResizeRectTransformWithTextureProportions(_itemImage.sprite, _changeImageSizeTo);
			_itemImage.color = Color.white;
			_amount.gameObject.SetActive(true);
			_amount.text = _inventoryItem.Value.ToString();

			return this;
		}

		public void Clear() {
			_itemImage.sprite = null;
			_itemImage.color = Color.clear;
			_inventoryItem = null;
			_amount.gameObject.SetActive(false);
		}
	}
}