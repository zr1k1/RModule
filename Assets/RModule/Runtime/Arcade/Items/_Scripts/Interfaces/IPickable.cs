using UnityEngine;

public interface IPickable {
	void PickUp(GameObject pickerGo);
	void Drop(GameObject dropperGo);
}
