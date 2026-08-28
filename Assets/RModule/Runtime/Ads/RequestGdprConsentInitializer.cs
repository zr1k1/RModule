#if USE_APPODEAL && USE_APPODEAL_CMP
using UnityEngine;
using System.Collections;
using AppodealStack.Cmp;
using AppodealStack.Monetization.Api;

namespace RModule.Runtime.AppodealHelper {
    public class RequestGdprConsentInitializer : Initializer {

        string _appodealAppKey;
        int _age;
        bool _requestFinished;
        bool _consentPopupShowFinished;
        ConsentForm _consentForm;

        public RequestGdprConsentInitializer(string appodealAppKey, int age = 14) {
            _appodealAppKey = appodealAppKey;
            _age = age;
            _requestFinished = false;
            _consentPopupShowFinished = false;
            _consentForm = null;
        }

        public override IEnumerator Initialize() {
            Debug.Log("RequestGdprConsentInitializer : Initialize");

            ConsentManager.Instance.OnConsentInfoUpdateSucceeded += (_, _) => {
                Debug.Log("RequestGdprConsentInitializer : OnConsentInfoUpdateSucceeded");

                Debug.Log(
                    $"RequestGdprConsentInitializer : ConsentStatus: {ConsentManager.Instance.ConsentStatus}"
                );

                _requestFinished = true;
            };

            ConsentManager.Instance.OnConsentInfoUpdateFailed += (_, args) => {
                Debug.Log(
                    $"RequestGdprConsentInitializer : OnConsentInfoUpdateFailed. Cause: {args.Cause}"
                );

                _requestFinished = true;
            };

            ConsentManager.Instance.OnConsentFormLoadSucceeded += (_, args) => {
                Debug.Log("RequestGdprConsentInitializer : OnConsentFormLoadSucceeded");

                _consentForm = args.ConsentForm;
                _requestFinished = true;
            };

            ConsentManager.Instance.OnConsentFormLoadFailed += (_, args) => {
                Debug.Log(
                    $"RequestGdprConsentInitializer : OnConsentFormLoadFailed. Cause: {args.Cause}"
                );

                _requestFinished = true;
            };

            ConsentManager.Instance.OnConsentFormDismissed += (_, args) => {
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

            ConsentManager.Instance.RequestConsentInfoUpdate(parameters);

            // Wait for RequestConsentInfoUpdate
            while (!_requestFinished)
                yield return null;

            var status = ConsentManager.Instance.ConsentStatus;

            Debug.Log($"RequestGdprConsentInitializer : ConsentStatus = {status}");

            // Consent is not required
            if (status != ConsentStatus.Required) {
                _consentPopupShowFinished = true;
                yield break;
            }

            // Load form
            _requestFinished = false;

            ConsentManager.Instance.Load();

            while (!_requestFinished)
                yield return null;

            // Show form
            if (_consentForm != null) {
                _consentPopupShowFinished = false;

                _consentForm.Show();

                while (!_consentPopupShowFinished)
                    yield return null;
            }
            yield return null;
        }
    }
}

#endif