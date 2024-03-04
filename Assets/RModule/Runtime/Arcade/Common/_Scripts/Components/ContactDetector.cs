using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public interface IContactDetector<T> : IStartContactDetector<T>, IEndContactDetector<T> {
}

public interface IStartContactDetector<T> {
	void OnStartContact(T contactedObject);
}

public interface IEndContactDetector<T> {
	void OnEndContact(T contactedObject);
}

public class ContactDetector : MonoBehaviour {

	// Events
	public UnityEvent<GameObject> DidStartContact = new UnityEvent<GameObject>();
	public UnityEvent<GameObject> DidEndContact = new UnityEvent<GameObject>();

	// Privats
	Action<Collider2D> _checkOnTriggerEnter2DStartContactDetection;
	Action<Collider2D> _checkOnTriggerExit2DStartContactDetection;
	Action<Collision2D> _checkOnColliderEnter2DStartContactDetection;
	Action<Collision2D> _checkOnColliderExit2DStartContactDetection;

	public void Setup<T>(T obj) {
		_checkOnTriggerEnter2DStartContactDetection = (collision) => {
			CheckStartContactDetection(obj, collision);
		};
		_checkOnTriggerExit2DStartContactDetection = (collision) => {
			CheckEndContactDetection(obj, collision);
		};
		_checkOnColliderEnter2DStartContactDetection = (collision) => {
			CheckStartContactDetection(obj, collision);
		};
		_checkOnColliderExit2DStartContactDetection = (collision) => {
			CheckEndContactDetection(obj, collision);
		};
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		_checkOnTriggerEnter2DStartContactDetection?.Invoke(collision);
	}

	private void OnTriggerExit2D(Collider2D collision) {
		_checkOnTriggerExit2DStartContactDetection?.Invoke(collision);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		_checkOnColliderEnter2DStartContactDetection?.Invoke(collision);
	}

	private void OnCollisionExit2D(Collision2D collision) {
		_checkOnColliderExit2DStartContactDetection?.Invoke(collision);
	}

	void CheckStartContactDetection<T>(T obj, Collider2D collision) {
		OnStartContact(obj, collision.gameObject);
	}

	void CheckEndContactDetection<T>(T obj, Collider2D collision) {
		OnEndContact(obj, collision.gameObject);
	}

	void CheckStartContactDetection<T>(T obj, Collision2D collision) {
		OnStartContact(obj, collision.gameObject);
	}

	void CheckEndContactDetection<T>(T obj, Collision2D collision) {
		OnEndContact(obj, collision.gameObject);
	}

	void OnStartContact<T>(T obj, GameObject userGo) {;
		var detectedObject = userGo.GetComponent<IStartContactDetector<T>>();
		if (detectedObject != null) {
			detectedObject.OnStartContact(obj);
			DidStartContact?.Invoke(userGo);
		}
	}

	void OnEndContact<T>(T obj, GameObject userGo) {
		var detectedObject = userGo.GetComponent<IEndContactDetector<T>>();
		if (detectedObject != null) {
			detectedObject.OnEndContact(obj);
			DidEndContact?.Invoke(userGo);
		}
	}
}