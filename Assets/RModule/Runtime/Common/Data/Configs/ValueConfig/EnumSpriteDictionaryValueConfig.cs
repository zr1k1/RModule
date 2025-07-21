using UnityEngine;
using System;

public class EnumSpriteDictionaryValueConfig<TEnum> : TypeSpriteDictionaryValueConfig<TEnum> where TEnum : Enum {
	public override bool GetSpriteByValue(TEnum value, out Sprite sprite) {
		sprite = null;
		if (_value.ContainsKey(value)) {
			sprite = _value[value];

			return true;
		}

		return false;
	}
}
