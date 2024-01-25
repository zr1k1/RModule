public interface IValueSaver<ValueType> {
	bool TrySave(ValueType value);
}