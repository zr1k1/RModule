#if USE_APPODEAL && USE_APPODEAL_CMP

using UnityEngine;
using System.Collections;
using AppodealStack.Cmp;
using AppodealStack.Monetization.Api;

namespace RModule.Runtime.AppodealHelper {

    public class RequestGdprConsentInitializer : Initializer {

        const float ConsentInfoTimeout = 3f;
        const float ConsentFormLoadTimeout = 3f;

        string _appodealAppKey;
        int _age;

        bool _requestFinished;
        bool _consentInfoRequestSucceeded;
        bool _consentPopupShowFinished;

        ConsentForm _consentForm;

        public RequestGdprConsentInitializer(string appodealAppKey, int age = 14) {
            _appodealAppKey = appodealAppKey;
            _age = age;
            _requestFinished = false;
            _consentInfoRequestSucceeded = false;
            _consentPopupShowFinished = false;
            _consentForm = null;
        }

        public override IEnumerator Initialize() {

            Debug.Log("RequestGdprConsentInitializer : Initialize");

            var consentManager = ConsentManager.Instance;

            consentManager.OnConsentInfoUpdateSucceeded += (_, _) => {
                Debug.Log("RequestGdprConsentInitializer : OnConsentInfoUpdateSucceeded");
                Debug.Log($"RequestGdprConsentInitializer : ConsentStatus: {consentManager.ConsentStatus}");
                _consentInfoRequestSucceeded = true;
                _requestFinished = true;
            };

            consentManager.OnConsentInfoUpdateFailed += (_, args) => {
                Debug.Log($"RequestGdprConsentInitializer : OnConsentInfoUpdateFailed. Cause: {args.Cause}");
                _consentInfoRequestSucceeded = false;
                _requestFinished = true;
            };

            consentManager.OnConsentFormLoadSucceeded += (_, args) => {
                Debug.Log("RequestGdprConsentInitializer : OnConsentFormLoadSucceeded");
                _consentForm = args.ConsentForm;
                _requestFinished = true;
            };

            consentManager.OnConsentFormLoadFailed += (_, args) => {
                Debug.Log($"RequestGdprConsentInitializer : OnConsentFormLoadFailed. Cause: {args.Cause}");
                _requestFinished = true;
            };

            consentManager.OnConsentFormDismissed += (_, args) => {
                string message = "RequestGdprConsentInitializer : OnConsentFormDismissed";

                if (args.Error != null)
                    message += $" Error: {args.Error}";

                Debug.Log(message);

                _consentForm = null;
                _consentPopupShowFinished = true;
            };

            var parameters = new ConsentInfoParameters {
                AppKey = _appodealAppKey,
                IsUnderAgeToConsent = _age <= 13,
                Sdk = "Appodeal",
                SdkVersion = Appodeal.GetNativeSDKVersion()
            };

            _requestFinished = false;
            _consentInfoRequestSucceeded = false;

            consentManager.RequestConsentInfoUpdate(parameters);

            float elapsed = 0f;

            while (!_requestFinished && elapsed < ConsentInfoTimeout) {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            if (!_requestFinished) {
                Debug.LogWarning($"RequestGdprConsentInitializer : Consent info update timeout after {ConsentInfoTimeout} seconds");
                _requestFinished = true;
                _consentInfoRequestSucceeded = false;
            }

            if (!_consentInfoRequestSucceeded) {
                Debug.LogWarning("RequestGdprConsentInitializer : Consent info request failed or timed out. Skipping consent form Load.");
                yield break;
            }

            var status = consentManager.ConsentStatus;

            Debug.Log($"RequestGdprConsentInitializer : ConsentStatus = {status}");

            if (status != ConsentStatus.Required) {
                Debug.Log("RequestGdprConsentInitializer : Consent is not required");
                yield break;
            }

            _requestFinished = false;
            _consentForm = null;

            consentManager.Load();

            elapsed = 0f;

            while (!_requestFinished && elapsed < ConsentFormLoadTimeout) {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            if (!_requestFinished) {
                Debug.LogWarning($"RequestGdprConsentInitializer : Consent form load timeout after {ConsentFormLoadTimeout} seconds");
                _requestFinished = true;
            }

            if (_consentForm != null) {
                Debug.Log("RequestGdprConsentInitializer : Showing consent form");

                _consentPopupShowFinished = false;

                _consentForm.Show();

                while (!_consentPopupShowFinished)
                    yield return null;
            }

            Debug.Log("RequestGdprConsentInitializer : Finished");

            yield return null;
        }
    }
}

#endif