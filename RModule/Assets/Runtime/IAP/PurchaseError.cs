namespace RModule.Runtime.IAP {
	public class PurchaseError {
		public enum ErrorType { UserCancelled = 0, IapNotInitialized, Other }

		public ErrorType Error { get; }
		public string Message { get; }

		public PurchaseError(ErrorType error, string message) {
			Error = error;
			Message = message;
		}
	}
}
