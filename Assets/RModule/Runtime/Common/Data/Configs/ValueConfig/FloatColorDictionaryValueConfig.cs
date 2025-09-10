using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "FloatColorDictionaryValueConfig", menuName = "RModule/Values/FloatColorDictionaryValueConfig", order = 1)]
public class FloatColorDictionaryValueConfig : ValueConfig<SerializableDictionary<float, Color>>, IValueByKeyGetter<float, Color> {

	public bool TryGetValue(float key, out Color value) {
		value = Color.white;
		if (_value.Count != 0) {
			int index = 0;
			for (int i = 0; i < _value.Count; i++) {
				if (key >= _value.ElementAt(i).Key) {
					value = _value.ElementAt(index).Value;
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
