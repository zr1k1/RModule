namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class BuffsController : MonoBehaviour, IBuffUser {
		// Outlets
		[SerializeField] protected Transform _buffsContainer = default;
		[SerializeField] protected List<Buff> _buffs = default;

		// Privats
		IBuffUser _buffUser;

		public void Setup(IBuffUser buffUser) {
			_buffUser = buffUser;
		}

		public void ApplyBuff(Buff buff) {
			buff.Setup(-1);
			buff.transform.SetParent(_buffsContainer, false);
			buff.DidEnd.AddListener(OnBuffEnded);
			_buffs.Add(buff);

		}

		public List<Buff> GetAllBuffs<T>() where T : IBuffEffect {
			return _buffs.FindAll(buff => buff.GetComponent<T>() != null);
		}

		public void OnBuffEnded(Buff buff) {
			_buffs.Remove(buff);
			buff.Delete();
		}
	}
}
