using System;
public interface IRewardedAdActionsProvider {
	bool RewardedAdIsReadyForShow(string placement);
	void ShowRewardedAd(string placement, Action<bool> finishCallback);
}
