using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ObjectRadar : MonoBehaviour {
	// Enums
	//public enum ObjectType { None, Dangerous }

	// Events
	public UnityEvent<ObjectRadar, GameObject> ObjectDidDetected = default;
	public UnityEvent<ObjectRadar, GameObject> ObjectDidUnDetected = default;

	// Accessors
	public List<GameObject> DetectedObjects => _detectedObjects;

	// Outlets
	[SerializeField] DetectionObjectTypeParametersDictionary _detectionObjectTypeParametersDictionary = default;
	[SerializeField] List<GameObject> _detectedObjects = default;

	// Privats

	// Classes
	[Serializable]
	public class DetectionObjectTypeParametersDictionary : SerializableDictionary<string, Parameters> { }

	[Serializable]
	public class Parameters {
		public int test;
	}

	void Start() {
		_detectedObjects = new List<GameObject>();
	}

	void OnTriggerEnter2D(Collider2D collision) {
		RemoveNulls();

		TryAddDetectedObject<IDangerousRadarObject>(collision.gameObject);
	}

	void OnTriggerExit2D(Collider2D collision) {
		RemoveNulls();
		if (_detectedObjects.Contains(collision.gameObject)) {
			_detectedObjects.Remove(collision.gameObject);
			ObjectDidUnDetected?.Invoke(this, collision.gameObject);
		}

	}

	void RemoveNulls() {
		_detectedObjects.RemoveAll((detectedObject) => detectedObject == null);
	}

	void TryAddDetectedObject<T>(GameObject detectedGo) where T : IRadarObject {
		if (_detectionObjectTypeParametersDictionary.ContainsKey(typeof(T).Name) && detectedGo.GetComponent<T>() != null) {
			_detectedObjects.Add(detectedGo);
			ObjectDidDetected?.Invoke(this, detectedGo);
			Debug.Log($"ObjectRadar : Object of type {typeof(T).Name} detected");
		}
	}
}
