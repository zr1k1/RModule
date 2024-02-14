using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.Arcade {
	public interface ITeleportable {
		bool CanTeleport();
		IEnumerator OnStartTeleport(TeleportBlock teleportBlock);
		void OnEndTeleport();
	}
}
