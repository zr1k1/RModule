public interface ISelectable {
	bool IsSelected { get; }

	void Deselect();
	void Select();
	void SetEnableSelection(bool enable);
	bool SelectionIsEnabled();
}
