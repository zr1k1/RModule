namespace RModule.Runtime.Services {
	public class AppPolicyService {
		public bool AppPolicyAccepted { get; private set; }
		public int UserAge { get; private set; }
		public bool HasTargetedAdsConsent { get; private set; }
		public string AppPolicyLink { get; private set; }
		public string AppTermsLink { get; private set; }

		// Const
		const string K_userAge = "LNP_userAge";
		const string K_appPolicyAccepted = "LNP_appPolicyAccepted";
		const string K_hasTargetedAdsConsent = "LNP_hasTargetedAdsConsent";

		readonly ISaveService _saveService;

		public class InputData {
			public ISaveService saveService;
			public string appPolicyLink;
			public string appTermsLink;
		}

		public AppPolicyService(InputData inputData) {
			_saveService = inputData.saveService;
			UserAge = _saveService.GetValue(K_userAge, 0);
			AppPolicyAccepted = _saveService.GetValue(K_appPolicyAccepted, false);
			HasTargetedAdsConsent = _saveService.GetValue(K_hasTargetedAdsConsent, false);
			AppPolicyLink = inputData.appPolicyLink;
			AppTermsLink = inputData.appTermsLink;
		}

		// ---------------------------------------------------------------
		// General methods

		public void SetAdsConsent(bool hasConsent) {
			if (HasTargetedAdsConsent != hasConsent) {
				HasTargetedAdsConsent = hasConsent;
				_saveService.SetValue(K_hasTargetedAdsConsent, HasTargetedAdsConsent);
				_saveService.Save();
			}
		}

		public void SetUserAge(int age) {
			if (age > 0 && age <= 99) {
				UserAge = age;
				_saveService.SetValue(K_userAge, UserAge);
				_saveService.Save();
			}
		}

		public bool UserIsChild => UserAge <= 13;

		public bool UserHasProvidedAge => UserAge != 0;

		public void AcceptAppPolicy() {
			AppPolicyAccepted = true;
			_saveService.SetValue(K_appPolicyAccepted, AppPolicyAccepted);
			_saveService.Save();
		}
	}
}
