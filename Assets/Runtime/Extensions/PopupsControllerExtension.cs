using System.Collections;
using UnityEngine;
using System;

public static class PopupsControllerExtension {
	//// Cosnts
	//const string k_reviewPopupAlreadyShown = "reviewPopupAlreadyShown";

	//public static void TryShowReviewPopup(this PopupsController popupsController, ReviewController reviewController) {
	//	Debug.Log("TryShowReviewPopup");
	//	if (!PlayerPrefsHelper.GetBool(k_reviewPopupAlreadyShown)) {
	//		var difference = GameDataManager.Instance.GameProgressData.LevelsPassed - SettingsManager.Instance.AppConfigData.FirstLevelToShowLikeGamePopup;
	//		if (difference >= 0 && difference % SettingsManager.Instance.AppConfigData.LikeGamePopupLevelsThreeshold == 0) { 

	//			popupsController.ShowReviewPopup(reviewController);
	//		}
	//	}
	//}

	//public static void ShowReviewPopup(this PopupsController popupsController, ReviewController reviewController) {
	//	Debug.Log("ShowReviewPopup");
	//	popupsController.Create<ReviewPopupVC>().Setup(() => {
	//		popupsController.ShowNativeReviewPopupOrOpenLink(reviewController);
	//	}).Show();
	//}

	//public static void ShowNativeReviewPopupOrOpenLink(this PopupsController popupsController, ReviewController reviewController) {
	//	PlayerPrefsHelper.SetBool(k_reviewPopupAlreadyShown, true);
	//	if (Application.platform == RuntimePlatform.Android)
	//		popupsController.StartCoroutine(ShowNativeReviewPopupOrOpenReviewLinkAfterDelay(reviewController, 0.4f));
	//	else
	//		reviewController.ShowNativeReviewPopupOrOpenReviewLink(true, true);
	//}

	//static IEnumerator ShowNativeReviewPopupOrOpenReviewLinkAfterDelay(ReviewController reviewController, float delay) {
	//	yield return new WaitForSeconds(delay);
	//	reviewController.ShowNativeReviewPopupOrOpenReviewLink(true, true);
	//}

	//public static void TryShowRemoveAdsOfferPopup(this PopupsController popupsController, Action yesBtnTappedCallback) {
	//	popupsController.Create<RemoveAdsOfferPopupVC>().Setup(yesBtnTappedCallback).Show();
	//}
}
