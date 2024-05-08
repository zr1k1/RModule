using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPersistentSavedDataConfig : MonoBehaviour {
	[SerializeField] PersistentSavedDataConfig<ExamplePlayerDataValues> playerPersistentDataConfig = default;

	private void Start() {
		var playerDataProvider = new DataProvider<ExamplePlayerDataValues, PersistentSavedDataConfig<ExamplePlayerDataValues>>(playerPersistentDataConfig);
		
		//DataProvider<ExamplePlayerDataValues, PersistentSavedDataConfig<ExamplePlayerDataValues>>.Reset(playerPersistentDataConfig);
		playerDataProvider.LoadData();
		var data = playerDataProvider.Data;
		foreach (var value in data.Values) {
			Debug.Log($"Test : {value.Key} {value.Value}");
		}
		Debug.LogError($"Test : Coins1 {(int)playerDataProvider.Data.GetValue<int>(ExamplePlayerDataValues.Coins)}");
		//data.SetValue(ExamplePlayerDataValues.Coins, 2);

		//data.SetValue(ExamplePlayerDataValues.DisableAds, true);
		//foreach (var value in data.Values) {
		//	Debug.Log($"Test : after set {value.Key} {value.Value}");
		//}
		//playerDataProvider.SaveData();
		//Debug.Log($"Test : SaveData");

		//var playerDataProvider1 = new DataProvider<ExamplePlayerDataValues, PersistentSavedDataConfig<ExamplePlayerDataValues>>(playerPersistentDataConfig);
		//playerDataProvider1.LoadData();
		//Debug.Log($"Test : LoadData");
		//var data1 = playerDataProvider1.Data;
		//foreach (var value in data1.Values) {
		//	Debug.Log($"Test : after load change save load {value.Key} {value.Value}");
		//}
		//Debug.LogError($"Test : Coins1 {(int)playerDataProvider.Data.GetValue<int>(ExamplePlayerDataValues.Coins)}");
	}
}
