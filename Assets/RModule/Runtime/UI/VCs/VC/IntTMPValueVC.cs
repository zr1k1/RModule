public class IntTMPValueVC : TMPValueVC<int> {

	public override void UpdateValue(int value) {
		base.UpdateValue(value);
		_valueLabel.text = _valueLabel.text.TryReplaceNum(_value, _symbolBeforeValue);
	}
}
