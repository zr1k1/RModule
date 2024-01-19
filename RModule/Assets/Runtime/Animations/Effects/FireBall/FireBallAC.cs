using System;
using System.Collections;
using UnityEngine;

public class FireBallAC : BaseAC {
	// Accessors
	public float Duration => _duration;

	// Outlets
	[SerializeField] ParticleSystem _particlesInMove = default;
	[SerializeField] ParticleSystem _particlesFireWork = default;
	[SerializeField] float _duration = 1;
	[SerializeField] float _moveTime = 1;
	[SerializeField] Material _material;
	[SerializeField] Sprite _particleSprite;

	//Private vars
	Vector2 _startPosTransform;
	Vector2 _endPosTransform;
	Material _usedMaterial;

	// ---------------------------------------------------------------
	// Setup

	public FireBallAC Setup(Vector3 startPosTransform, Vector3 endPosTransform, Action moveEndedCallback) {
		base.SetupAnimation(null);

		_startPosTransform = startPosTransform;
		_endPosTransform = endPosTransform;
		DidEndCallback.AddListener(moveEndedCallback.Invoke);
		SetParticleMaterial(_material);

		return this;
	}

	// Override animation behavior
	protected override IEnumerator Animate() {
		var particlesInMoveRT = _particlesInMove.GetComponent<RectTransform>();
		particlesInMoveRT.position = _startPosTransform;
		_particlesInMove.Play();
		yield return LeanTween.move(particlesInMoveRT.gameObject, _endPosTransform, _moveTime);
		yield return new WaitForSeconds(_moveTime);
		_particlesInMove.Stop();
		var particlesFireWorkRT = _particlesFireWork.GetComponent<RectTransform>();
		particlesFireWorkRT.position = _endPosTransform;
		_particlesFireWork.Play();
		yield return new WaitForSeconds(_particlesFireWork.main.duration);
		yield return base.Animate();
	}

	public void SetParticleMaterial(Material material) {
		if (_usedMaterial != null)
			Destroy(_usedMaterial);

		_material = material;
		_usedMaterial = Instantiate(_material);
		var particleSystemRenderer = _particlesInMove.gameObject.GetComponent<ParticleSystemRenderer>();
		_usedMaterial.mainTexture = _particleSprite.texture;
		particleSystemRenderer.material = _usedMaterial;
		particleSystemRenderer = _particlesFireWork.gameObject.GetComponent<ParticleSystemRenderer>();
		particleSystemRenderer.material = _usedMaterial;
	}

	public void SetParticleSprite(SpriteRenderer spriteRenderer) {
		_particleSprite = spriteRenderer.sprite;
		if(_usedMaterial != null)
			_usedMaterial.mainTexture = _particleSprite.texture;
		_particlesInMove.transform.localScale = new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 1f);
		_particlesFireWork.transform.localScale = new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 1f);
	}
}
