using RModule.Runtime.Arcade;
using RModule.Runtime.Sounds;
using UnityEngine;

public class ExampleGirlFinishBlock : FinishBlock {

	[Header("Main Animation")]
	[SerializeField] Animator _animator = default;
	[SerializeField] string _girlAnimTrigger = default;

	[Header("Bubble Animation")]
	[SerializeField] Animator _batteryAnimator = default;

	[Header("Hearts Animation")]
	[SerializeField] ParticleSystem _heartsAnimPs = default;

	// Privats

	public override void PlayAnimation() {
		_animator.SetTrigger(_girlAnimTrigger);
		_batteryAnimator.SetTrigger(_girlAnimTrigger);
	}

	public void PlayHeartsAnim() {
		_heartsAnimPs.Play();
	}

	public void PlaySound(SoundConfig soundConfig) {
		SoundsManager.Instance.PlaySoundEffect(soundConfig);
	}
}
