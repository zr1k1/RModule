namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using System;

	public interface IBuffUser {
		public void ApplyBuff(Buff buff);
	}

	public class Buff : LevelElement {
		// Events
		public UnityEvent<Buff> DidStart = default;
		public UnityEvent<Buff> DidEnd = default;

		// Privats
		int _remainingTime;
		bool _infinityTime;

		public virtual Buff Setup(float timeInSeconds) {
			_infinityTime = timeInSeconds == -1;

			DidStart?.Invoke(this);
			StartCoroutine(StartTimer());

			return this;
		}

		IEnumerator StartTimer() {
			while (_remainingTime != 0) {
				if (_remainingTime > -1) {
					yield return new WaitForSeconds(1);
					_remainingTime -= 1;
				} else if (_remainingTime == 0) {
					DidEnd?.Invoke(this);
				}
			}
		}

		public void Cancel() {
			_remainingTime = 0;
		}

		public virtual void Delete() {
			Destroy(gameObject);
		}
	}
}