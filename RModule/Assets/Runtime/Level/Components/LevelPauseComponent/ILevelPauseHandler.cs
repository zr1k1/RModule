using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelPauseHandler {
	void OnLevelPause();
	void OnLevelResume();
}
