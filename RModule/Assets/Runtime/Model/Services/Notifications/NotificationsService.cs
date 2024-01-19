using System;
using System.Collections.Generic;
using RModule.Runtime.Plugins.Notifications;
using UnityEngine;

[Serializable]
public class NotificationsService<T> where T : Enum {
	// Outlets
	[SerializeField] GameNotificationsManager _gameNotificationsManager = default;
	[SerializeField] NotificationsConfig<T> _notificationsConfig = default;

	public void Initialize() {
		if (_gameNotificationsManager.Initialized)
			return;
				
		var channel = new GameNotificationChannel(_notificationsConfig.IdGameNotificationChannel
			, _notificationsConfig.NameGameNotificationChannel
			, _notificationsConfig.DescriptionGameNotificationChannel);

		_gameNotificationsManager.Initialize(channel);
	}

	public void ScheduleNotifications(T notificationName) {
		if (!TryGetNotificationData(notificationName , out var notificationData))
			return;

		if (notificationData.CancelIfAlreadyInSchedule)
			CancelNotifications(notificationName);

		var scheduleDeliveryTimeInSecondsList = notificationData.GetScheduleDeliveryTimeInSecondsList();
		for (int i = 0; i < scheduleDeliveryTimeInSecondsList.Count; i++)
			ScheduleNotification(notificationName, notificationData, i);
	}

	public void CancelNotifications(T notificationName) {
		if (!TryGetNotificationData(notificationName, out var notificationData))
			return;

		var scheduleDeliveryTimeInSecondsList = notificationData.GetScheduleDeliveryTimeInSecondsList();
		for (int i = 0; i < scheduleDeliveryTimeInSecondsList.Count; i++)
			_gameNotificationsManager.CancelNotification(GetScheduledNotificationId(notificationName, i));
	}

	void ScheduleNotification(T notificationName, NotificationData notificationData, int indexInScheduleList) {
		int deliveryTimeInSeconds = notificationData.GetScheduleDeliveryTimeByIndex(indexInScheduleList);
		if (!_gameNotificationsManager.Initialized || notificationData == null || deliveryTimeInSeconds <= 0)
			return;

		var notification = _gameNotificationsManager.CreateNotification();
		if (notification == null)
			return;

		notification.Title = notificationData.Title;
		notification.Body = notificationData.GetRandomText();
		notification.Group = _notificationsConfig.IdGameNotificationChannel;
		notification.DeliveryTime = DateTime.Now.ToLocalTime() + TimeSpan.FromSeconds(deliveryTimeInSeconds);
		notification.SmallIcon = notificationData.SmallIcon;
		notification.LargeIcon = notificationData.LargeIcon;
		notification.BadgeNumber = notificationData.BadgeNumber;

		var notificationToDisplay = _gameNotificationsManager.ScheduleNotification(notification);
		notificationToDisplay.Reschedule = false;

		if (notificationToDisplay.Notification.Id != null) {
			SetScheduledNotificationId(notificationName, indexInScheduleList, notificationToDisplay.Notification.Id.Value);
		}
	}

	int GetScheduledNotificationId(T notificationName, int indexInScheduleList) {
		return PlayerPrefs.GetInt(uniqPrefsKey(notificationName, indexInScheduleList));
	}

	void SetScheduledNotificationId(T notificationName, int indexInScheduleList, int value) {
		PlayerPrefs.SetInt(uniqPrefsKey(notificationName, indexInScheduleList), value);
		PlayerPrefs.Save();
	}

	bool TryGetNotificationData(T notificationName, out NotificationData notificationData) {
		return _notificationsConfig.TryGetNotificationData(notificationName, out notificationData);
	}

	string uniqPrefsKey(T notificationName, int indexInScheduleList) {
		return $"{notificationName}_{indexInScheduleList}";
	}
}

