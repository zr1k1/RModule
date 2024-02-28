using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable {
	bool TryGetMoveEndPoint(out Vector3 moveEndPoint);
}
