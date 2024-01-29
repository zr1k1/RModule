namespace RModule.Runtime.Arcade {

	public interface IItemContactHandler {
		void OnStartContactWithItem(Item item);
		void OnEndContactWithItem(Item item);
	}
}
