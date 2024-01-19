using UnityEngine;

namespace RModule.Runtime.Utils {
	public static class DeviceInfoHelper {
		
		public static string GetAboutDeviceString() {
			string osVersion = SystemInfo.operatingSystem;
			string deviceModel = SystemInfo.deviceModel;
			string gameTitle = Application.productName;
			string appVersion = Application.version;

			string aboutString = $"{gameTitle} ({appVersion}, {osVersion}, {deviceModel})";

#if UNITY_ANDROID && USE_HUAWEI_SERVICES
			aboutString += " - AG";
#elif UNITY_ANDROID && GALAXY_STORE_BUILD
			aboutString += " - SMG";
#endif
			
			return aboutString;
		}
	}
}