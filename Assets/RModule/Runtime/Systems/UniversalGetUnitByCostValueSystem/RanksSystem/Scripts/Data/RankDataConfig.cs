using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//todo just prepared to realize
[CreateAssetMenu(fileName = "RankDataConfig", menuName = "RModule/Systems/UniversalGetUnitByCostValueSystem/Ranks/Main/RankDataConfig", order = 1)]
public class RankDataConfig : BaseUnitByCostDataConfig<float> {
    public int Id => _id;
    public Sprite EmptyImg => _emptyImg;
    public Sprite FullImg => _fullImg;
    public string LocalizedName => LocalizedText.T($"{_commonLocalizedNameKey}{_id}");

    [Header("Rank")]
    [SerializeField] protected int _id = default;
    [SerializeField] protected Sprite _emptyImg = default;
    [SerializeField] protected Sprite _fullImg = default;
    [SerializeField] protected string _commonLocalizedNameKey = "kRankId";

    public override bool TryGet(float valueToGet, out float remainder) {
        remainder = valueToGet;
        if (valueToGet >= GetCost()) {
            remainder = GetCost() - valueToGet;

            return true;
        }

        return false;
    }
}
