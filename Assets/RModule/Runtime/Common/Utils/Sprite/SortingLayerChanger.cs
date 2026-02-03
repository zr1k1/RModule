using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortingLayerChanger {

    SerializableDictionary<Transform, string> _savedSortingLayersNames = new();
    Transform _transform;

    public SortingLayerChanger(Transform transform) {
        _transform = transform;
    }

    public void ChangeSortingLayer(string sortingLayerName) {
        _savedSortingLayersNames.Clear();
        TryChangeOnChilds(_transform, sortingLayerName);

    }

    public void TurnBack() {
        TryTurnBackOnChilds(_transform);

        _savedSortingLayersNames.Clear();
    }

    void TryChangeOnChilds(Transform tr, string sortingLayerName) {
        var sprRend = tr.GetComponent<SpriteRenderer>();
        if (sprRend != null) {
            _savedSortingLayersNames.Add(tr, sprRend.sortingLayerName);
            sprRend.sortingLayerName = sortingLayerName;
        }

        foreach (Transform child in tr) {
            TryChangeOnChilds(child, sortingLayerName);
        }
    }

    void TryTurnBackOnChilds(Transform tr) {
        var sprRend = tr.GetComponent<SpriteRenderer>();
        if (sprRend != null) {
            sprRend.sortingLayerName = _savedSortingLayersNames[tr];
        }

        foreach (Transform child in tr) {
            TryTurnBackOnChilds(child);
        }
    }

}
