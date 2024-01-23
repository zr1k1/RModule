using System;

namespace RModule.Runtime.Services {
	public interface ISaveService : ISaveServiceValue<bool>, ISaveServiceValue<int>, ISaveServiceValue<float>, ISaveServiceValue<string>, ISaveServiceValue<DateTime> {
		void Save();
	}
}
