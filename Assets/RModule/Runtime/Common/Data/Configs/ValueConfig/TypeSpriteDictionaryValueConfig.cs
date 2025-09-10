using UnityEngine;

public abstract class TypeSpriteDictionaryValueConfig<TKey> : ValueConfig<SerializableDictionary<TKey, Sprite>>, ISpriteGetterByValue<TKey> {
	public abstract bool TryGetValue(TKey key, out Sprite sprite);
}
