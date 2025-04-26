using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseGetUnitsConfig<TCost, TUnitByCostData, TOutData> : ScriptableObject
	where TCost : IComparable
	where TUnitByCostData : BaseUnitByCostDataConfig<TCost>
	where TOutData : OutData<TCost> { 

	[SerializeField] protected List<TUnitByCostData> _unitDataList = default;

	public virtual bool TryGetIndexInList(TUnitByCostData unit, out int index) {
		index = _unitDataList.IndexOf(unit);

		return index >= 0;
	}

	public virtual List<TOutData> GetCanBeGettedUnitDataList(int startIndex, TCost valueToGet, out TCost valueRemainder) {
		int index = startIndex;
		valueRemainder = valueToGet;
		List<TOutData> gettedUnitDataList = new List<TOutData>();
		while (TryGetUnit(index, valueToGet, out var getUnitData, out var remainder)) {
			valueToGet = remainder;
			gettedUnitDataList.Add(getUnitData);
			index++;
		}

		return gettedUnitDataList;
	}

	public virtual bool TryGetNextUnit(TUnitByCostData unit, TCost valueToGet, out TOutData outData, out TCost valueRemainder) {
		outData = default(TOutData);
		valueRemainder = valueToGet;
		if (TryGetIndexInList(unit, out var index)) {
			var nextIndex = index + 1;
			return TryGetUnit(nextIndex, valueToGet, out outData, out valueRemainder);
		}

		return false;
	}

	protected virtual bool TryGetUnit(int index, TCost valueToGet, out TOutData outData, out TCost valueRemainder){
		outData = default(TOutData);
		valueRemainder = valueToGet;

		if (index < _unitDataList.Count) {
			if (_unitDataList[index].TryGet(valueToGet, out valueRemainder)) {
				outData = default(TOutData);
				outData.cost = _unitDataList[index].GetCost();

				return true;
			}
		} 

		return false;
	}

}
