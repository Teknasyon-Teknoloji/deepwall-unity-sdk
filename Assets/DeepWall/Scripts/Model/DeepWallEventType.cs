
namespace DeepWallModule
{
    public class DeepWallEventType
    {
        // Common events
        public const string PaywallRequested = "deepWallPaywallRequested";
        public const string PaywallResponseReceived = "deepWallPaywallResponseReceived";
        public const string PaywallResponseFailure = "deepWallPaywallResponseFailure";
        public const string PaywallOpened = "deepWallPaywallOpened";
        public const string PaywallNotOpened = "deepWallPaywallNotOpened";
        public const string PaywallActionShowDisabled = "deepWallPaywallActionShowDisabled";
        public const string PaywallClosed = "deepWallPaywallClosed";
        public const string PaywallExtraDataReceived = "deepWallPaywallExtraDataReceived";
        public const string PaywallPurchasingProduct = "deepWallPaywallPurchasingProduct";
        public const string PaywallPurchaseSuccess = "deepWallPaywallPurchaseSuccess";
        public const string PaywallPurchaseFailed = "deepWallPaywallPurchaseFailed";
        public const string PaywallRestoreSuccess = "deepWallPaywallRestoreSuccess";
        public const string PaywallRestoreFailed = "deepWallPaywallRestoreFailed";

        // Android ONLY events
        public const string PaywallConsumeSuccess = "deepWallPaywallConsumeSuccess";
        public const string PaywallConsumeFail = "deepWallPaywallConsumeFailure";
    }
}