public interface IAppPolicyService  {
	public bool AppPolicyAccepted { get; }
	public int UserAge { get; }
	public bool HasTargetedAdsConsent { get; }
}
