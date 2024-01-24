using System;
using System.Collections;
using System.Collections.Generic;

[Serializable] public class ValuesData<T0, T1, T2> : SerializableDictionary<T0, T1> where T1 : ValueData<T2> { }

[Serializable] public class BoolValuesData<T> : ValuesData<T, ValueData<bool>, bool> { }
[Serializable] public class IntValuesData<T> : ValuesData<T, ValueData<int>, int> { }
[Serializable] public class StringValuesData<T> : ValuesData<T, ValueData<string>, string> { }
[Serializable] public class FloatValuesData<T> : ValuesData<T, ValueData<float>, float> { }
[Serializable] public class ListValuesData<T0, T1> : ValuesData<T0, ValueData<List<T1>>, List<T1>> { }
[Serializable] public class DictionaryValuesData<T0, T1, T2> : ValuesData<T0, ValueData<SerializableDictionary<T1, T2>>, SerializableDictionary<T1, T2>> { }

[Serializable] public class BoolChangeableValuesData<T> : ValuesData<T, ChangeableValueData<bool>, bool> { }
[Serializable] public class IntChangeableValuesData<T> : ValuesData<T, ChangeableValueData<int>, int> { }
[Serializable] public class StringChangeableValuesData<T> : ValuesData<T, ChangeableValueData<string>, string> { }
[Serializable] public class FloatChangeableValuesData<T> : ValuesData<T, ChangeableValueData<float>, float> { }
