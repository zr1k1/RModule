using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableAssetProvider {
	// Accessors
	public GameObject AddressableDataTempGameObjectContainer => _tempAddressableDataContainer;

	// Private vars
	string _address;
	AssetReference _assetReference;
	GameObject _tempAddressableDataContainer;
	AutoReleaseAddressableOnDisable autoReleaseAddressableOnDisable;

	static Transform s_parentForTempGameObjects;

	// Consts
	const string c_tempAddressablesGameObjectsContainerName = "tempAddressablesGameObjectsContainer";
	const string c_tempAddressableDataContainerName = "tempAddressableDataContainer";

	public void LoadAsset<T>(string address, Action<T> completedCallback) {
		//Debug.LogError($"AddressableAssetProvider : LoadAsset");
		CreateTempGameObjectWithAddressableProvider().TryGetObject(address, completedCallback);
	}

	public void LoadAsset<T>(AssetReference assetReference, Action<T> completedCallback) {
		CreateTempGameObjectWithAddressableProvider().TryGetObject(assetReference, completedCallback);
	}

	AddressableAssetProvider CreateTempGameObjectWithAddressableProvider() {
		if (s_parentForTempGameObjects == null)
			s_parentForTempGameObjects = new GameObject(c_tempAddressablesGameObjectsContainerName).transform;

		_tempAddressableDataContainer = new GameObject(c_tempAddressableDataContainerName, typeof(AutoReleaseAddressableOnDisable));
		autoReleaseAddressableOnDisable = _tempAddressableDataContainer.GetComponent<AutoReleaseAddressableOnDisable>();

		_tempAddressableDataContainer.transform.SetParent(s_parentForTempGameObjects);

		return this;
	}

	void TryGetObject<T>(string address, Action<T> completeCallback) {
		_address = address;
		var asyncOperationHandleSprite = Addressables.LoadAssetAsync<T>(_address);
		autoReleaseAddressableOnDisable._asyncOperationHandle = asyncOperationHandleSprite;
		autoReleaseAddressableOnDisable._asyncOperationHandle.Completed += handle => {
			if (handle.Status == AsyncOperationStatus.Succeeded) {
				if (handle.Result == null) {
					Debug.LogError($"Not exist object");
				} else {
					completeCallback.Invoke((T)handle.Result);
				}
			} else {
				Debug.LogError($"Failed to load addressable object");
			}
		};
	}

	void TryGetObject<T>(AssetReference assetReference, Action<T> completeCallback) {
		_assetReference = assetReference;
		var asyncOperationHandleSprite = _assetReference.LoadAssetAsync<T>();
		autoReleaseAddressableOnDisable._asyncOperationHandle = asyncOperationHandleSprite;
		autoReleaseAddressableOnDisable._asyncOperationHandle.Completed += handle => {
			if (handle.Status == AsyncOperationStatus.Succeeded) {
				if (handle.Result == null) {
					Debug.LogError($"Not exist object");
				} else {
					completeCallback.Invoke((T)handle.Result);
				}
			} else {
				Debug.LogError($"Failed to load addressable object");
			}
		};
	}

	public class AutoReleaseAddressableOnDisable : MonoBehaviour {
		public AsyncOperationHandle _asyncOperationHandle;

		private void OnDisable() {
			Addressables.Release(_asyncOperationHandle);
			Resources.UnloadUnusedAssets();
		}
	}
}
