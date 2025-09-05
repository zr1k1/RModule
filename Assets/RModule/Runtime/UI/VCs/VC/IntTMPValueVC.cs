public class IntTMPValueVC : TMPValueVC<int> {

	public override void UpdateValue(int value) {
		base.UpdateValue(value);
		if (string.IsNullOrEmpty(_symbolBeforeValue)) {
			_valueLabel.text = _value.ToString();
		} else {
			_valueLabel.text = _valueLabel.text.TryReplaceNum(_value, _symbolBeforeValue);
		}
	}
}
