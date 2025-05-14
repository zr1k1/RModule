using System;

public interface IValueByEnumGetter<TValue, TEnumType> where TEnumType : Enum {
	TValue GetValue(TEnumType enumType);
}
