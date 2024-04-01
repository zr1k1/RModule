namespace RModule.Runtime.Arcade.Inventory {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class InventoryVC : MonoBehaviour {

		[SerializeField] RectTransform _cellsContainer = default;
		[SerializeField] ItemCellInventoryVC _cellPrefab = default;

		// Privats
		List<ItemCellInventoryVC> _itemCellInventoryVCs = new List<ItemCellInventoryVC>();

		public void Setup(InventoryController inventoryController) {
			for (int i = 0; i < inventoryController.Size; i++) {
				var cell = Instantiate(_cellPrefab, _cellsContainer);
				_itemCellInventoryVCs.Add(cell);
			}

			OnUpdateView(inventoryController);
			inventoryController.DidUpdate.AddListener(OnUpdateView);
		}

		public void OnUpdateView(InventoryController inventoryController) {
			foreach (var cell in _itemCellInventoryVCs)
				cell.Clear();

			gameObject.SetActive(inventoryController.InventoryItems.Count != 0);

			for (int i = 0; i < inventoryController.InventoryItems.Count; i++) {
				var item = inventoryController.InventoryItems[i];
				_itemCellInventoryVCs[i].Setup(item);
			}
		}
	}
}
