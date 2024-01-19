using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AnimationsManager : MonoBehaviour {
	// Outlets
	[Header("Setup"), Space]
	[SerializeField] Transform _animationsParent = default;

	[Header("Animation prefabs"), Space]
	[SerializeField] List<BaseAC> _animationsPrefabs = default;

	// Private vars
	List<BaseAC> _animations = new List<BaseAC>();

	public T Create<T>() where T : BaseAC {
		Assert.IsNotNull(_animationsParent, "Animations's parent must not be null");

		foreach (var popupPrefab in _animationsPrefabs) {
			if (popupPrefab is T prefab) {
				var animationController = Instantiate(prefab, _animationsParent);
				_animations.Add(animationController);
				animationController.AddDidEndCallback(()=> { RemoveFromListWhenDestroy(animationController); });

				return animationController;
			}
		}

		return null;
	}

	public int GetIsPlayingCount<T>() where T : BaseAC {
		return _animations.FindAll(animation => animation is T).Count;
 	}

	void RemoveFromListWhenDestroy<T>(T animationController) where T : BaseAC {
		if (_animations.Contains(animationController))
			_animations.Remove(animationController);
	}
}
