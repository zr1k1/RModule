using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SpriteOffset : MonoBehaviour {

	[SerializeField] SpriteRenderer _spriteRenderer = default;

	public Vector2 offset = default;
	
	private void Update() {
		_spriteRenderer.material.SetTextureOffset("_MainTex", offset);
	}
}
