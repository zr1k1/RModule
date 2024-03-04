namespace RModule.Runtime.Arcade.Inventory {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.Linq;

	public class InventoryVC : MonoBehaviour {

		[SerializeField] Transform _itemsContainer = default;
		[SerializeField] List<Item> _items = default;

		// Privats
		int _size;

		public InventoryVC Setup(int size = -1) {
			_size = size;

			return this;
		}

		public bool TryAddItem(Item item) {
			Debug.Log($"InventoryVC : TryAddItem{item.GetType()}");
			if (_items.Count <= _size || _size == -1) {
				_items.Add(item);
				//item.gameObject.SetActive(false);
				item.transform.SetParent(_itemsContainer);
				//item.transform.localPosition = new Vector2(0, _items.Count - 1);
				item.GetComponent<Collider2D>().enabled = false;
				UpdateListView();

				return true;
			}

			return false;
		}

		public void RemoveItem(Item item) {
			_items.Remove(item);
			UpdateListView();
		}

		public List<Item> GetAllItemByType<T>(T item) {
			Debug.Log($"InventoryVC : item type {item.GetType()}");
			UpdateListView();

			return _items.FindAll(item => item is T);
		}

		public List<T> GetAllItemByType<T>() where T : Item {
			//Debug.Log($"InventoryVC : item type {item.GetType()}");
			UpdateListView();

			return _items.OfType<T>().ToList();
		}

		public void UpdateListView() {
			Debug.Log($"InventoryVC : UpdateListView");
			//_items.RemoveAll(item => item == null);
			for (int i = 0; i < _items.Count; i++) {
				_items[i].transform.localPosition = new Vector2(-(float)(_items.Count / 2f)+ 0.5f + (float)i, -1);
			}
		}
	}
}