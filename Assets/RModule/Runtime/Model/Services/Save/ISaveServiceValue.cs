namespace RModule.Runtime.Services {
	public interface ISaveServiceValue<T> {
		void SetValue(string key, T value);
		T GetValue(string key, T defaultValue = default);
	}
}
