using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NotificationData {
	// Accessors
	public string IdKey => _idKey;
	public string Title => _title;
	public string SmallIcon => _smallIcon;
	public string LargeIcon => _largeIcon;
	public int BadgeNumber => _badgeNumber;
	public bool CancelIfAlreadyInSchedule => _cancelIfAlreadyInSchedule;

	// Outlets
	[Tooltip("Uniq key for save to prefs, example project_comeBackToTheGameNotificationId")]
	[SerializeField] string _idKey = default;
	[Tooltip("example Evo Puzzle")]
	[SerializeField] string _title = default;
	[SerializeField] List<string> _bodyTextLocalizationKeys = default;
	[SerializeField] DeliveryTime _deliveryTime = default;
	[SerializeField] List<int> _scheduleDeliveryTimesList = default;
	[Tooltip("example icon_0, but use different, cause icon will be white square")]
	[SerializeField] string _smallIcon = "icon_0";
	[SerializeField] string _largeIcon = "icon_1";
	[Tooltip("example 1")]
	[SerializeField] int _badgeNumber = 1;
	[SerializeField] bool _cancelIfAlreadyInSchedule = default;

	public string GetRandomText() {
		return LocalizedText.T(_bodyTextLocalizationKeys[UnityEngine.Random.Range(0, _bodyTextLocalizationKeys.Count)]);
	}

	public List<int> GetScheduleDeliveryTimeInSecondsList() {
		var deliveryTimeInSecondsList = new List<int>();
		var totalModifier = 1;
		var minutsModifier = 60;
		var hoursModifier = minutsModifier * 60;
		var daysModifier = hoursModifier * 24;
		var weeksModifier = daysModifier * 7;

		foreach (var deliveryTime in _scheduleDeliveryTimesList) {
			if (_deliveryTime == DeliveryTime.Minutes) {
				totalModifier = minutsModifier;
			} else if (_deliveryTime == DeliveryTime.Hours) {
				totalModifier = hoursModifier;
			} else if (_deliveryTime == DeliveryTime.Days) {
				totalModifier = daysModifier;
			} else if (_deliveryTime == DeliveryTime.Weeks) {
				totalModifier = weeksModifier;
			}

			deliveryTimeInSecondsList.Add(deliveryTime * totalModifier);
		}

		return deliveryTimeInSecondsList;
	}

	public int GetScheduleDeliveryTimeByIndex(int indexInScheduleDeliveryTimesList) {
		var scheduleDeliveryTimeInSecondsList = GetScheduleDeliveryTimeInSecondsList();
		if (indexInScheduleDeliveryTimesList < scheduleDeliveryTimeInSecondsList.Count)
			return GetScheduleDeliveryTimeInSecondsList()[indexInScheduleDeliveryTimesList];
		else
			return -1;
	}
}
