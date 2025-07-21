using UnityEngine;

public interface ISpriteGetterByValue<T> {
    bool GetSpriteByValue(T value, out Sprite sprite);
}
