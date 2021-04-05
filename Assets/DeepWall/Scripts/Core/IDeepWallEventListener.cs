namespace DeepWallModule
{
    public interface IDeepWallEventListener
    {
        /// <summary>
        /// Fired after paywall requested. Useful for displaying loading indicator in your app.
        /// </summary>
        void OnDeepWallPaywallRequested();

        //Fired after paywall response received. Useful for hiding loading indicator in your app.
        void DeepWallPaywallResponseReceived();

        //Paywall opened event
        void DeepWallPaywallOpened(int pageId);

        //Paywall not opened event. Fired on error cases only.
        void DeepWallPaywallNotOpened(int pageId);

        //Paywall action show disabled event.
        void DeepWallPaywallActionShowDisabled(int pageId);

        //Paywall closed event
        void DeepWallPaywallClosed(int pageId);

        //Paywall purchasing product event
        void DeepWallPaywallPurchasingProduct(string productCode);

        //Paywall response failure event
        void DeepWallPaywallResponseFailure(string errorCode, string reason);

        //Purchase success event. Fired after receipt validation if Ploutos service active.
        void DeepWallPaywallPurchaseSuccess(int type, string result);

        //Purchase failed event
        void DeepWallPaywallPurchaseFailed(int productCode, string reason, string errorCode, bool isPaymentCancelled);

        //Restore success event
        void DeepWallPaywallRestoreSuccess();

        //Restore failed event
        void DeepWallPaywallRestoreFailed(int productCode, string reason, string errorCode, bool isPaymentCancelled);

        //Extra data received event
        void DeepWallPaywallExtraDataReceived(string data);

        void DeepWallPaywallConsumeSuccess(string data);

        void DeepWallPaywallConsumeFail(string data);
    }
}