using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IAttachableSurface : ISortable {
	event Action DidDestroyed;

	bool CanBeAttached(GameObject go);
	void Attach(GameObject go);
	void Detach(GameObject go);
}
