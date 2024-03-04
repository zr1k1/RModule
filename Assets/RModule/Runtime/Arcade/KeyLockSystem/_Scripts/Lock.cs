using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RModule.Runtime.Arcade;
using RModule.Runtime.Arcade.Inventory;

public class Lock : UnPickableItem, IStartContactDetector<Key> {
	// Enums
	public enum MaterialType { Steel = 0 }

	// Accessors
	public ColorTypeEnum Color => _colorType;
	public bool IsUnlocked => _isUnlocked;

	// Outlets
	[SerializeField] List<Sprite> _coloredLockSprites = default;
	[SerializeField] ColorTypeEnum _colorType = default;

	[SerializeField] List<Lock> _locks = default;
	[SerializeField] UnityEvent DidUnlocked = default;

	// Privats
	bool _isUnlocked;

	protected override void Awake() {
		base.Awake();
		if (_colorType == ColorTypeEnum.Invisible) {
			p_spriteRenderer.enabled = false;
			p_collider2D.enabled = false;
		} else
			p_spriteRenderer.sprite = _coloredLockSprites[(int)_colorType];
	}

	protected override void Start() {
		p_contactDetector.Setup(this);
	}

	public void TryUnlock() {
		if (_isUnlocked)
			return;

		if (_locks.Count == 0 || _locks.FindAll(lock1 => !lock1.IsUnlocked).Count == 0) {
			_isUnlocked = true;
			DidUnlocked?.Invoke();
			Destroy();
		}
	}

	public override void Destroy() {
		base.Destroy();

		enabled = false;
		p_collider2D.enabled = false;
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		Destroy(gameObject, 5);
	}

	public void OnStartContact(Key contactedObject) {
		if (_colorType == contactedObject.Color && !IsUnlocked) {
			TryUnlock();
			if (IsUnlocked)
				contactedObject.Destroy();
		}
	}
}
