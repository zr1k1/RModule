using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableAssetProvider {
	// Private vars
	string _address;
	AssetReference _assetReference;

	public AsyncOperationHandle<T> LoadAsset<T>(string address, Action<T> completedCallback) {
		return TryGetObject(address, completedCallback);
	}

	public AsyncOperationHandle<T> LoadAsset<T>(AssetReference assetReference, Action<T> completedCallback) {
		return TryGetObject(assetReference, completedCallback);
	}

	AsyncOperationHandle<T> TryGetObject<T>(string address, Action<T> completeCallback) {
		_address = address;
		var asyncOperationHandleSprite = Addressables.LoadAssetAsync<T>(_address);
		asyncOperationHandleSprite.Completed += handle => {
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

		return asyncOperationHandleSprite;
	}

	AsyncOperationHandle<T> TryGetObject<T>(AssetReference assetReference, Action<T> completeCallback) {
		_assetReference = assetReference;
		var asyncOperationHandleSprite = _assetReference.LoadAssetAsync<T>();
		asyncOperationHandleSprite.Completed += handle => {
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

		return asyncOperationHandleSprite;
	}

}

public class AutoReleaseAddressableOnDisable : MonoBehaviour {
	public AsyncOperationHandle asyncOperationHandle = default;

	private void OnDisable() {
		Addressables.Release(asyncOperationHandle);
	}
}
