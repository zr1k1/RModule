public interface IKeyValueGetter<Key, Value> {
	Value GetValue(Key key, Value defaultValue = default);
}
