public class FloatTMPValueVC : TMPValueVC<float> {

	public override void UpdateValue(float value) {
		base.UpdateValue(value);
		_valueLabel.text = _valueLabel.text.TryReplaceNum(_value, _symbolBeforeValue);
	}
}
