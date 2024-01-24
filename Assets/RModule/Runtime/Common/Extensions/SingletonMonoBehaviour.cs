using System.Collections;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour, IInitializable where T : Component {
	public static T Instance => _instance as T;

	static T _instance;

	// Do NOT use Awake method in the derived classes or it will not be called here
	// Use OnSingletonAwake() method instead
	void Awake() {
		if (_instance == null) {
			_instance = this as T;
			transform.parent = null;
			DontDestroyOnLoad(gameObject);
			OnSingletonAwake();
		} else if (_instance != this) {
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// A substitute for the Awake() method. Will be called after all of the singleton stuff is done
	/// </summary>
	protected virtual void OnSingletonAwake() { }

	public static IEnumerator WaitForInstanceCreatedAndInitialized() {
		while (!InstanceCreatedAndInitialized())
			yield return null;
	}

	public static bool InstanceCreatedAndInitialized() {
		return Instance != null && ((IInitializable)Instance).IsInitialized();
	}

	public abstract bool IsInitialized();

	public IEnumerator WaitForInitialized() {
		while (!IsInitialized())
			yield return null;
	}
}