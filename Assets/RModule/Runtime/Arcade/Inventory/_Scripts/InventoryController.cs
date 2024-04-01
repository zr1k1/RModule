namespace RModule.Runtime.Arcade.Inventory {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using System.Linq;
	using System;

	public class InventoryController : MonoBehaviour {
		// Events
		public UnityEvent<InventoryController> DidUpdate = default;

		// Accessors
		public List<InventoryItem> InventoryItems => _inventoryItems;
		public int Size => _size;

		// Outlets
		[SerializeField] Transform _itemsContainer = default;
		[SerializeField] List<InventoryItem> _inventoryItems = default;

		// Privats
		int _size;

		public interface ISummable {
			bool TrySum<T>(Item item, T value);
		}

		// Classes
		public abstract class BaseInventoryItem<T> : ISummable {
			public Item Item => _item;
			public T Value => _value;

			protected Item _item;
			protected T _value;

			public BaseInventoryItem(Item item, T value) {
				_item = item;
				_value = value;
			}

			public bool ItemsTypesIsEquals(Item item) {
				return _item.GetType() == item.GetType();
			}

			public abstract bool TrySum<T1>(Item item, T1 value);
		}

		[Serializable]
		public class InventoryItem : BaseInventoryItem<object> {
			public InventoryItem(Item item, object value = null) : base(item, value) {
			}

			public override bool TrySum<T1>(Item item, T1 value) {
				if (ItemsTypesIsEquals(item)) {
					return true;
				} else {
					return false;
				}
			}
		}

		[Serializable]
		public class IntInventoryItem : InventoryItem {

			public IntInventoryItem(Item item, object value) : base(item, value) {
			}

			public override bool TrySum<T>(Item item, T value) {
				if (ItemsTypesIsEquals(item)) {
					_value = (int)_value + (int)(object)value;
					return true;
				} else {
					return false;
				}
			}
		}

		[Serializable]
		public class FloatInventoryItem : InventoryItem, ISummable {
			public FloatInventoryItem(Item item, object value) : base(item, value) {
			}

			public override bool TrySum<T>(Item item, T value) {
				Debug.LogError($"InventoryVC : TrySum ");
				if (ItemsTypesIsEquals(item)) {
					_value = (float)_value + (float)(object)value;
					return true;
				} else {
					return false;
				}
			}
		}

		public InventoryController Setup(int size = -1) {
			Debug.Log($"InventoryController : Setup");
			_size = size;
			_inventoryItems = new List<InventoryItem>();

			return this;
		}

		public bool TryAddItem(Item item) {
			Debug.Log($"InventoryController : TryAddItem{item.GetType()}");
			if (_inventoryItems.Count <= _size || _size == -1) {
				var inventoryItem = new InventoryItem(item);
				var iValueableInt = item.GetComponent<IValueable<int>>();
				var iValueableFloat = item.GetComponent<IValueable<float>>();
				if (iValueableInt != null) {
					inventoryItem = new IntInventoryItem(item, ((IValueable<int>)item).GetValue());
				} else if (iValueableFloat != null) {
					inventoryItem = new FloatInventoryItem(item, ((IValueable<float>)item).GetValue());
				}

				if (inventoryItem is IntInventoryItem intInventoryItem) {
					var allIntInventoryItems = _inventoryItems.OfType<IntInventoryItem>().ToList();
					IntInventoryItem inventoryItemWithSameType = allIntInventoryItems.Find(inventoryItem => inventoryItem.ItemsTypesIsEquals(item));
					if (inventoryItemWithSameType == null) {
						_inventoryItems.Add(intInventoryItem);
					} else {
						if (((IValueable<int>)item) != null) {
							inventoryItemWithSameType.TrySum(item, ((IValueable<int>)item).GetValue());
						} 
						Destroy(item.gameObject);
					}

				}
				else if (inventoryItem is FloatInventoryItem) {
					// TODO when need
				} else {
					_inventoryItems.Add(inventoryItem);
				}

				var foundedInventoryItem = _inventoryItems.Find(inventoryItem => inventoryItem.Item == item);

				item.transform.SetParent(_itemsContainer);
				UpdateListView();

				return true;
			}

			return false;
		}

		public void RemoveItem(Item item) {
			var foundedInventoryItem = _inventoryItems.Find(inventoryItem => inventoryItem.Item == item);
			if (foundedInventoryItem != null)
				_inventoryItems.Remove(foundedInventoryItem);
			UpdateListView();
		}

		public List<T> GetAllItemByType<T>() where T : Item {
			return _inventoryItems.Select(inventoryItem => inventoryItem.Item).OfType<T>().ToList();
		}

		public void UpdateListView() {
			Debug.Log($"InventoryController : UpdateListView");
			DidUpdate?.Invoke(this);
		}

		public bool TryChangeItemValue(Item item, int amount) {
			//Debug.LogError($"InventoryVC : TryChangeItemValue ");
			var foundedInventoryItem = _inventoryItems.Find(inventoryItem => inventoryItem.Item == item) as IntInventoryItem;
			if (foundedInventoryItem != null) {
				var iValueableInt = foundedInventoryItem.Item.GetComponent<IValueable<int>>();
				if (iValueableInt != null) {
					var totalValue = (int)foundedInventoryItem.Value + amount;
					if (totalValue >= 0) {
						if (foundedInventoryItem is IntInventoryItem intInventoryItem) {
							intInventoryItem.TrySum(foundedInventoryItem.Item, amount);
						}
						if (totalValue == 0) {
							foundedInventoryItem.Item.Destroy();
						}

						DidUpdate?.Invoke(this);
						return true;
					}
				} else {
					Debug.LogError($"InventoryController : foundedItem {foundedInventoryItem.Item.name} is not implement IValueable interface");
				}
			}

			return false;
		}
	}
}