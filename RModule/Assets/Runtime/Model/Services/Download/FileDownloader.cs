
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace RModule.Runtime.Services {
	public class FileDownloader {
		
		// Private vars
		readonly string _fileUrl;
		readonly string _fileLocalPath;
		Action<float> _progressUpdateCallback;
		UnityWebRequest _webRequest;

		// ---------------------------------------------------------------
		// Constructor
		
		public FileDownloader(string fileUrl, string fileLocalPath) {
			_fileUrl = fileUrl;
			_fileLocalPath = fileLocalPath;
		}

		~FileDownloader() {
			CancelDownload();
		}
		
		// ---------------------------------------------------------------
		// General methods

		public void Download(Action<float> progressUpdateCallback, Action<bool> finishCallback) {
			_progressUpdateCallback = progressUpdateCallback;
			
			_webRequest = new UnityWebRequest(_fileUrl);
			_webRequest.downloadHandler = new DownloadHandlerBuffer();
			
			var downloadOperation = _webRequest.SendWebRequest();
			downloadOperation.completed += operation => {
				if (operation.isDone && _webRequest.result == UnityWebRequest.Result.Success) {
					if (_webRequest != null) {
						File.WriteAllBytes(_fileLocalPath, _webRequest.downloadHandler.data);
						Debug.Log($"Downloaded file to path: {_fileLocalPath}");
						_webRequest = null;
						finishCallback?.Invoke(true);
					}
				} else {
					_webRequest = null;
					finishCallback?.Invoke(false);
				}
			};
		}

		public void CancelDownload() {
			if (_webRequest != null) {
				_webRequest.Abort();
				_webRequest.Dispose();
			}

			_progressUpdateCallback = null;
			_webRequest = null;
		}
		
		// ---------------------------------------------------------------
		// Update

		public void Update() {
			if (_webRequest != null) {
				_progressUpdateCallback?.Invoke(_webRequest.downloadProgress);
			}
		}
	}
}