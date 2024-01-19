using System;
using System.Collections.Generic;
using UnityEngine;

public class NotificationsConfig<OptionalNotificatioName> : ScriptableObject {

	// Accessors
	public string IdGameNotificationChannel => _idGameNotificationChannel;
	public string NameGameNotificationChannel => _nameGameNotificationChannel;
	public string DescriptionGameNotificationChannel => _descriptionGameNotificationChannel;

	// Outlets
	[Tooltip("Example, evopuzzle_notification_channel_id_0")]
	[SerializeField] string _idGameNotificationChannel = default;
	[Tooltip("Example, Evo Puzzle Notifications")]
	[SerializeField] string _nameGameNotificationChannel = default;
	[Tooltip("Example, Evo Puzzle channel")]
	[SerializeField] string _descriptionGameNotificationChannel = default;
	//[SerializeField] List<NotificationData> _notificationDatas = default;
	[SerializeField] SerializableDictionary<OptionalNotificatioName, NotificationData> _notificationDatasDictionary = default;

	public bool TryGetNotificationData(OptionalNotificatioName optionalNotificatioName, out NotificationData notificationData) {
		notificationData = null;
		if (_notificationDatasDictionary.ContainsKey(optionalNotificatioName)) {
			notificationData = _notificationDatasDictionary[optionalNotificatioName];
			return true;
		} else {
			Debug.LogError($"Key {optionalNotificatioName} is not present on _notificationDatasDictionary dictionary");
			return false;
		}
	}
}