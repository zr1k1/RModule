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
			Debug.Log($"InventoryVC : Setup {inventoryController.Size}");
			for (int i = 0; i < inventoryController.Size; i++) {
				var cell = Instantiate(_cellPrefab, _cellsContainer);
				cell.Clear();
				_itemCellInventoryVCs.Add(cell);
			}

			inventoryController.DidUpdate.AddListener(OnUpdateView);
		}

		public void OnUpdateView(InventoryController inventoryController) {
			foreach (var cell in _itemCellInventoryVCs)
				cell.Clear();

			for (int i = 0; i < inventoryController.InventoryItems.Count; i++) {
				var item = inventoryController.InventoryItems[i];
				_itemCellInventoryVCs[i].Setup(item);
			}
		}
	}
}
