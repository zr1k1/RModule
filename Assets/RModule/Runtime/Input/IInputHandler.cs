using System;
using UnityEngine;

public interface IInputHandler {
	event Action DidClick;
	event Action DidBeginDrag;
	event Action DidOnDrag;
	event Action DidEndDrag;
	event Action DidDown;
	event Action DidUp;

	Canvas Canvas { get; }
	Camera UICamera { get; }
}
