using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocationGraphicKitsManager {
	void AddLocationElement(ILocationGraphicKitsElement locationGraphicKitsElement);
	void RemoveLocationElement(ILocationGraphicKitsElement locationGraphicKitsElement);
	void UpdateElementsView();
	string GetLocationSpriteAddress(string key);
}
