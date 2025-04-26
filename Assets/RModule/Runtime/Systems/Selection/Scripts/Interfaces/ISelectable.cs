public interface ISelectable {
	void Deselect();
	bool IsSelected { get; }
	void Select();
	void SetEnableSelection(bool enable);
}
