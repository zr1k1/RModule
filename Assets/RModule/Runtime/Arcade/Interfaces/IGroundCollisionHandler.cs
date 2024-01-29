using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGroundCollisionHandler {
	void OnGroundEnter(GameObject ground);
	void OnGroundExit(GameObject ground);
}
