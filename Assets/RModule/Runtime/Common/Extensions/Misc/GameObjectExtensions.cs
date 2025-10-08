using UnityEngine;
using System;
using System.Linq;

public static class GameObjectExtensions {

    public static void RemoveAllMonoBehaviours(this GameObject go, params Type[] exceptionTypes) {
        var exceptionTypesList = exceptionTypes.ToList();
        var monoBehaviours = go.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var mb in monoBehaviours) {
            if (mb == null)
                continue;
            if (!exceptionTypesList.Contains(mb.GetType()))
                GameObject.Destroy(mb);
        }
    }
}
