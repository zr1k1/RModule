using System;
using System.Collections;

public class ExampleSettingsManager : BaseSettingsManager<ExampleGameItemToPurchase, ExamplePlacementType, ExampleOptionalAppConfigValue, ExmapleOptionalSetting, ExampleDebugValue> {
	public override IEnumerator Initialize(ISoundsPlayerService soundsPlayerService, Action<bool> setEnableVibration) {

		UpdateNubmerOfStars();
		UpdateLastPlayedDay();
		UpdatePlayedDaysInARowCount();

		return base.Initialize(soundsPlayerService, setEnableVibration);
	}

	void UpdateNubmerOfStars() {
		SetValue(CommonSetting.NumberOfStarts, GetValue<int>(CommonSetting.NumberOfStarts) + 1);
	}

	void UpdateLastPlayedDay() {
		SetValue(CommonSetting.LastPlayedDay, DateTime.Now);
	}

	void UpdatePlayedDaysInARowCount() {
		if ((GetValue<DateTime>(CommonSetting.LastPlayedDay) - DateTime.Now).Days <= 1) {
			SetValue(CommonSetting.PlayedDaysInARowCount, GetValue<int>(CommonSetting.PlayedDaysInARowCount) + 1);
		}
	}
}
