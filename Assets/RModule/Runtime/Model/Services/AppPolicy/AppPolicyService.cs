namespace RModule.Runtime.Services {
	public class AppPolicyService {
		public bool AppPolicyAccepted => _saveService.GetValue(K_appPolicyAccepted, false);
		public int UserAge => _saveService.GetValue(K_userAge, 0);
		public bool HasTargetedAdsConsent => _saveService.GetValue(K_hasTargetedAdsConsent, false);
		public string AppPolicyLink => _inputData.appPolicyLink;
		public string AppTermsLink => _inputData.appTermsLink;

		// Const
		const string K_userAge = "LNP_userAge";
		const string K_appPolicyAccepted = "LNP_appPolicyAccepted";
		const string K_hasTargetedAdsConsent = "LNP_hasTargetedAdsConsent";

		readonly ISaveService _saveService;
		InputData _inputData;

		public class InputData {
			public ISaveService saveService;
			public string appPolicyLink;
			public string appTermsLink;
		}

		public AppPolicyService(InputData inputData) {
			_inputData = inputData;
			_saveService = _inputData.saveService;
		}

		// ---------------------------------------------------------------
		// General methods

		public void SetAdsConsent(bool hasConsent) {
			if (HasTargetedAdsConsent != hasConsent) {
				_saveService.SetValue(K_hasTargetedAdsConsent, hasConsent);
				_saveService.Save();
			}
		}

		public void SetUserAge(int age) {
			if (age > 0 && age <= 99) {
				_saveService.SetValue(K_userAge, age);
				_saveService.Save();
			}
		}

		public bool UserIsChild => UserAge <= 13;

		public bool UserHasProvidedAge => UserAge != 0;

		public void AcceptAppPolicy() {
			_saveService.SetValue(K_appPolicyAccepted, true);
			_saveService.Save();
		}
	}
}
