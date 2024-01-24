using System;

namespace RModule.Runtime.Services {
	public interface ISaveService :
		IKeyValueSetter<string, bool>, IKeyValueGetter<string, bool>, IKeyValueSetter<string, int>, IKeyValueGetter<string, int>,
		IKeyValueSetter<string, float>, IKeyValueGetter<string, float>, IKeyValueSetter<string, string>, IKeyValueGetter<string, string>,
		IKeyValueSetter<string, DateTime>, IKeyValueGetter<string, DateTime> {
		void Save();
	}
}
