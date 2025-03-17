public interface ISelectable {
	void Deselect();
	bool IsSelected { get; }
	void Select();
}
