using System.Collections;

public interface IInitializable {
	bool IsInitialized();
	IEnumerator WaitForInitialized();
}
