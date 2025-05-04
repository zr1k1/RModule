using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RModule.Runtime.LeanTween;

public class LightTailAC : BaseAC {

	// Accessors
	public float Duration => _duration;

	// Outlets
	[SerializeField] ParticleSystem _particlesInMove = default;
	[SerializeField] ParticleSystem _particlesFireWork = default;
	[SerializeField] bool _outStartScale = true;
	[SerializeField] bool _inStartScale = true;
	[SerializeField] bool _outEndScale = true;
	[SerializeField] bool _inEndScale = true;
	[SerializeField] LeanTweenType _scaleLeanTweenType = default;
	[SerializeField] float _scaleToModifier = default;
	[SerializeField] float _scaleTime = 0.5f;
	[SerializeField] float _duration = 1;
	[SerializeField] float _moveTime = 1;
	[SerializeField] Material _material;
	[SerializeField] Sprite _inMoveSprite;
	[SerializeField] Sprite _fireWorkSprite;
	[SerializeField] SpriteRenderer _tail = default;

	//Private vars
	Vector2 _startPosTransform;
	Vector2 _endPosTransform;
	Material _inMoveMaterial;
	Material _fireWorkMaterial;
	Vector2 _moveDirection;

	// ---------------------------------------------------------------
	// Setup

	public LightTailAC Setup(Vector3 startPosTransform, Vector3 endPosTransform, Action moveEndedCallback) {
		base.SetupAnimation(null);

		_startPosTransform = startPosTransform;
		_endPosTransform = endPosTransform;
		DidEndCallback.AddListener(moveEndedCallback.Invoke);

		SetParticleMaterial(_material);
		SetupLightTail();

		return this;
	}

	void SetupLightTail() {
		if (_tail == null)
			return;

		_tail.gameObject.SetActive(false);
		_moveDirection = _endPosTransform - _startPosTransform;
		var angle = -Vector2.SignedAngle(_moveDirection.normalized, Vector2.right);
		_tail.transform.localEulerAngles = new Vector3(0, 0, angle);
	}

	// Override animation behavior
	protected override IEnumerator Animate() {
		var particlesFireWorkRT = _particlesFireWork.GetComponent<RectTransform>();
		var particlesInMoveRT = _particlesInMove.GetComponent<RectTransform>();
		particlesFireWorkRT.position = _startPosTransform;
		particlesInMoveRT.position = _startPosTransform;
		_particlesFireWork.Play();
		_particlesInMove.Play();

		Vector2 startScale = particlesInMoveRT.localScale;
		Vector2 endScale = startScale * _scaleToModifier;
		if (_outStartScale) {
			yield return LeanTween.scale(particlesInMoveRT.gameObject, endScale, _scaleTime).setEase(_scaleLeanTweenType);
			yield return new WaitForSeconds(_scaleTime);
		}
		if (_inStartScale) {
			yield return LeanTween.scale(particlesInMoveRT.gameObject, startScale, _scaleTime).setEase(_scaleLeanTweenType);
			yield return new WaitForSeconds(_scaleTime);
		}
		SetActiveTail(true);
		LeanTween.move(particlesInMoveRT.gameObject, _endPosTransform, _moveTime);
		yield return new WaitForSeconds(_moveTime);
		_particlesInMove.Stop();
		SetActiveTail(false);

		if (_outEndScale) {
			yield return LeanTween.scale(particlesInMoveRT.gameObject, endScale, _scaleTime).setEase(_scaleLeanTweenType);
			yield return new WaitForSeconds(_scaleTime);
		}
		if (_inEndScale) {
			yield return LeanTween.scale(particlesInMoveRT.gameObject, startScale, _scaleTime).setEase(_scaleLeanTweenType);
			yield return new WaitForSeconds(_scaleTime);
		}

		yield return base.Animate();
	}

	public void SetActiveTail(bool isActive) {
		if (_tail == null)
			return;
		_tail?.gameObject.SetActive(isActive);
	}

	public void SetParticleMaterial(Material material) {
		if (_inMoveMaterial != null)
			Destroy(_inMoveMaterial);

		_material = material;
		_inMoveMaterial = Instantiate(material);
		_fireWorkMaterial = Instantiate(material);
		var particleSystemRenderer = _particlesInMove.gameObject.GetComponent<ParticleSystemRenderer>();
		_inMoveMaterial.mainTexture = _inMoveSprite.texture;
		particleSystemRenderer.material = _inMoveMaterial;
		particleSystemRenderer = _particlesFireWork.gameObject.GetComponent<ParticleSystemRenderer>();
		particleSystemRenderer.material = _fireWorkMaterial;
		_inMoveMaterial.mainTexture = _inMoveSprite.texture;
		_fireWorkMaterial.mainTexture = _fireWorkSprite.texture;
	}

	public void SetParticleMaterial(Material material, Color color) {
		SetParticleMaterial(material);
	}

	public void SetInMoveParticleSprite(SpriteRenderer spriteRenderer) {
		_inMoveSprite = spriteRenderer.sprite;
		if (_inMoveMaterial != null) {
			_inMoveMaterial.mainTexture = _inMoveSprite.texture;
			_inMoveMaterial.color = spriteRenderer.color;
		}
		_particlesInMove.transform.localScale = new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 1f);
	}

	public void SetFireWorkParticleSprite(Sprite sprite) {
		_fireWorkSprite = sprite;
		if (_fireWorkMaterial != null)
			_fireWorkMaterial.mainTexture = _inMoveSprite.texture;
	}
}
