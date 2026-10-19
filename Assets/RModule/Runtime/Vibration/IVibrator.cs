namespace RModule.Runtime.Vibration {

    public interface IVibrator {
        void VibrateShort();
        void VibrateTick();
        void VibrateCustom(long milliseconds);
        void VibratePattern(long[] pattern, int repeat);
        void Cancel();
    }

}
