using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Arcade;
using RModule.Runtime.Arcade.Inventory;

public class Key : PickedAtHandItem {
	// Enums
	public enum MaterialType { Steel = 0 }

	// Accessors
	public ColorTypeEnum Color => _colorType;

	// Outlets
	[SerializeField] List<Sprite> _coloredKeySprites = default;
	[SerializeField] ColorTypeEnum _colorType = default;

	protected override void Awake() {
		base.Awake();
		if (_colorType == ColorTypeEnum.Invisible)
			p_spriteRenderer.enabled = false;
		else
			p_spriteRenderer.sprite = _coloredKeySprites[(int)_colorType];
	}

	protected override void Start() {
		p_contactDetector.Setup(this);
	}

	public override void Destroy() {
		base.Destroy();

		enabled = false;
		p_collider2D.enabled = false;
		Destroy(gameObject, 0);
	}
}
