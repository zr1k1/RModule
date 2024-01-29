using System.Collections;

public interface IDoorAnimationComponent {
	IEnumerator OpenAnimation();
	IEnumerator CloseAnimation();
}
