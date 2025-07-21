using UnityEngine;

public abstract class TypeSpriteDictionaryValueConfig<TKey> : ValueConfig<SerializableDictionary<TKey, Sprite>>, ISpriteGetterByValue<TKey> {
	public abstract bool GetSpriteByValue(TKey value, out Sprite sprite);
}
