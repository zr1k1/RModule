using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using RModule.Runtime.Arcade
using RModule.Runtime.Utils;

[Serializable]
public class DataTMPValueVCsDictionary<T, T1> : SerializableDictionary<T, TMPValueVC<T1>> where T : Enum {
	public virtual void UdateView(IValueGetterByEnum<T> valueGetter) {
		foreach (var keyPair in this) {
			keyPair.Value.UpdateValue(valueGetter.GetValue<T1>(keyPair.Key));
		}
	}
}
