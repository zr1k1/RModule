using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "FloatSpriteDictionaryValueConfig", menuName = "RModule/Values/FloatSpriteDictionaryValueConfig", order = 1)]
public class FloatSpriteDictionaryValueConfig : TypeSpriteDictionaryValueConfig<float> {

	public override bool TryGetValue(float key, out Sprite sprite) {
		sprite = null;
		if (_value.Count != 0) {
			int index = 0;
			for(int i = 0; i < _value.Count; i++) {
				if(key >= _value.ElementAt(i).Key) {
					sprite = _value.ElementAt(index).Value;
				} else {
					break;
				}
				index++;
			}

			return true;
		} else {
			Debug.LogError($"FloatSpriteDictionaryValueConfig : Sprites is not setuped ");
		}

		return false;
	}
}
