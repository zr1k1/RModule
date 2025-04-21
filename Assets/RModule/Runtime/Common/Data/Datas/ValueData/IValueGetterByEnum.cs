using System;

public interface IValueGetterByEnum<T0> where T0 : Enum {
	T1 GetValue<T1>(T0 enumType);
}
