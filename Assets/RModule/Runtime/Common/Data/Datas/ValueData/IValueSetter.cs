public interface IValueSetter<TEnum> {
	void SetValue<T1>(TEnum enumType, T1 value);
}