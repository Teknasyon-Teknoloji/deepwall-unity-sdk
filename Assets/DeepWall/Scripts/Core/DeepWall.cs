using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

namespace DeepWallModule
{
    public class DeepWall : MonoBehaviour
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _InitDeepWall(string apiKey, int env);

        [DllImport("__Internal")]
        private static extern void _SetUserProperties(string uuid, string country, string language, int envStyle);

        [DllImport("__Internal")]
        private static extern void _UpdateUserProperties(string uuid, string country, string language, int envStyle);

        [DllImport("__Internal")]
        private static extern void _RequestPaywall(string action, [CanBeNull] string extraData);

        [DllImport("__Internal")]
        private static extern void _RequestAppTracking(string action, [CanBeNull] string extraData);

        [DllImport("__Internal")]
        private static extern void _SendExtraDataToPaywall(string extraData);

        [DllImport("__Internal")]
        private static extern void _ClosePaywall();

        [DllImport("__Internal")]
        private static extern void _ShowAlertMessage(string message);

#elif UNITY_ANDROID
        private static AndroidJavaObject androidDeepWall;
        private const string AndroidDeepWallPackage = "com.deepwall.plugin.unity.DeepWallUnity";
        private const string AndroidInitDeepWallMethod = "initDeepWall";
        private const string AndroidSetUserPropertiesMethod = "setUserProperties";
        private const string AndroidUpdateUserPropertiesMethod = "updateUserProperties";
        private const string AndroidRequestPaywallMethod = "requestPaywall";
        private const string AndroidClosePaywallMethod = "closePaywall";
        private const string AndroidSetProductUpgradePolicyMethod = "setProductUpgradePolicy";
        private const string AndroidUpdateProductUpgradePolicyMethod = "updateProductUpgradePolicy";
        private const string AndroidConsumeProductMethod = "consumeProduct";
#endif
        private const string DeepWallGameObjectName = "DeepWall";

        private static DeepWall _instance;
        private static string _apiKey;
        private static byte _environment;
        [CanBeNull] private static IDeepWallEventListener _eventListener;

        private static bool _dontDestroyOnLoad = true;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                if (_dontDestroyOnLoad)
                    DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
#if UNITY_IOS
            _InitDeepWall(_apiKey, _environment);
#elif UNITY_ANDROID
            AndroidJavaObject context = DeepWallUtils.GetUnityActivity();
            AndroidJavaObject application = context.Call<AndroidJavaObject>("getApplication");
            androidDeepWall = new AndroidJavaObject(AndroidDeepWallPackage);
            androidDeepWall.Call<string>(AndroidInitDeepWallMethod, _apiKey, (int) _environment);
#endif
        }

        /// <summary>
        /// Initialize Deepwall. Before calling any method of Deepwall make sure you initialized it successfully.
        /// When you call the Init function the observers will start immediately.
        /// This instantiate a DeepWall GameObject automatically if it is not already instantiated in your scene. 
        /// </summary>
        public static void InitDeepWall(string androidApiKey, string iosApiKey, DeepWallEnvironment environment,
            [CanBeNull] IDeepWallEventListener deepWallEventListener, bool dontDestroyOnLoad = true)
        {
#if UNITY_IOS
            DeepWall._apiKey = iosApiKey;
#elif UNITY_ANDROID
            DeepWall._apiKey = androidApiKey;
#endif
            DeepWall._environment = (byte) environment;
            DeepWall._eventListener = deepWallEventListener;
            DeepWall._dontDestroyOnLoad = dontDestroyOnLoad;

            GameObject dwGo = new GameObject(DeepWallGameObjectName);
            dwGo.AddComponent<DeepWall>();
        }

        /// <summary>
        /// Call this method for setting up user properties. Before requesting any paywall you need to set user properties.
        /// </summary>
        public static void SetUserProperties(DeepWallUserProperty userProperty)
        {
#if UNITY_IOS
            _SetUserProperties(userProperty.UUID, userProperty.Country, userProperty.Language,
                (int) userProperty.EnvironmentStyle);
#elif UNITY_ANDROID
            androidDeepWall.Call(AndroidSetUserPropertiesMethod, userProperty.UUID, userProperty.Country,
                userProperty.Language,
                (int) userProperty.EnvironmentStyle);
#endif
        }

        /// <summary>
        /// Call this method if any of userProperties is changed
        /// </summary>
        public static void UpdateUserProperties(DeepWallUserProperty userProperty)
        {
#if UNITY_IOS
            _UpdateUserProperties(userProperty.UUID, userProperty.Country, userProperty.Language,
                (int) userProperty.EnvironmentStyle);
#elif UNITY_ANDROID
            androidDeepWall.Call(AndroidUpdateUserPropertiesMethod, userProperty.Country, userProperty.Language,
                (int) userProperty.EnvironmentStyle);
#endif
        }

        /// <summary>
        /// Call this method to request and show Paywall dialog. 
        /// </summary>
        public static void RequestPaywall(string actionKey, [CanBeNull] string extraData)
        {
#if UNITY_IOS
            _RequestPaywall(actionKey, extraData);
#endif
#if UNITY_ANDROID
            androidDeepWall.Call(AndroidRequestPaywallMethod, actionKey, extraData);
#endif
        }

        /// <summary>
        /// Call this method for showing ATT prompts.
        /// </summary>
        public static void RequestAppTracking(string actionKey, [CanBeNull] string extraData)
        {
#if UNITY_IOS
            _RequestAppTracking(actionKey, extraData);
#endif
#if UNITY_ANDROID
            androidDeepWall.Call(AndroidRequestPaywallMethod, actionKey, extraData);
#endif
        }

        /// <summary>
        /// Call this method for sending extradata to Paywall. [iOS Only]
        /// </summary>
        public static void SendExtraDataToPaywall(string extraData)
        {
#if UNITY_IOS
            _SendExtraDataToPaywall(extraData);
#endif
        }

        /// <summary>
        /// Call this method to close Paywall dialog.
        /// </summary>
        public static void ClosePaywall()
        {
#if UNITY_IOS
            _ClosePaywall();
#endif
#if UNITY_ANDROID
            androidDeepWall.Call(AndroidClosePaywallMethod);
#endif
        }


        /// <summary>
        /// Call this method to handle subscription upgrade or downgrade processes. [Android Only]
        /// </summary>
        public static void SetProductUpgradePolicy(ProrationType prorationType, PurchaseUpgradePolicyType upgradePolicy)
        {
#if UNITY_ANDROID
            androidDeepWall.Call(AndroidSetProductUpgradePolicyMethod, (int) prorationType, (int) upgradePolicy);
#endif
        }

        /// <summary>
        /// Call this method to update the product upgrade policy of your paywalls. [Android Only]
        /// </summary>
        public static void UpdateProductUpgradePolicy(ProrationType prorationType,
            PurchaseUpgradePolicyType upgradePolicy)
        {
#if UNITY_ANDROID
            androidDeepWall.Call(AndroidUpdateProductUpgradePolicyMethod, (int) prorationType, (int) upgradePolicy);
#endif
        }

        /// <summary>
        /// Call this method to consume your products. [Android Only]
        /// </summary>
        public static void ConsumeProduct(string productId)
        {
#if UNITY_ANDROID
            androidDeepWall.Call(AndroidConsumeProductMethod, productId);
#endif
        }

        public static void ShowAlertMessage(string message)
        {
#if UNITY_IOS
            _ShowAlertMessage(message);
#endif
#if UNITY_ANDROID
#endif
        }

        public static void SetEventListener([CanBeNull] IDeepWallEventListener listener)
        {
            _eventListener = listener;
        }

        public static void RemoveEventListener()
        {
            _eventListener = null;
        }

        #region NATIVE_LEVEL_EVENTS

        /// <summary>
        /// Fired after paywall requested. Useful for displaying loading indicator in your app.
        /// </summary>
        public void DeepWallPaywallRequested()
        {
            _eventListener?.OnDeepWallPaywallRequested();
        }

        //Fired after paywall response received. Useful for hiding loading indicator in your app.
        public void DeepWallPaywallResponseReceived()
        {
            _eventListener?.DeepWallPaywallResponseReceived();
        }

        //Paywall response failure event
        public void DeepWallPaywallResponseFailure(string eventData)
        {
            DeepWallPaywallResponseFailureModel paywallResponseFailureResponse =
                JsonUtility.FromJson<DeepWallPaywallResponseFailureModel>(eventData);
            string errorCode = paywallResponseFailureResponse.errorCode;
            string reason = paywallResponseFailureResponse.reason;
            _eventListener?.DeepWallPaywallResponseFailure(errorCode, reason);
        }

        //Paywall opened event
        public void DeepWallPaywallOpened(string eventData)
        {
            int pageId = JsonUtility.FromJson<DeepWallCommonResponseModel>(eventData).pageId;
            _eventListener?.DeepWallPaywallOpened(pageId);
        }

        //Paywall not opened event. Fired on error cases only.
        public void DeepWallPaywallNotOpened(string eventData)
        {
            int pageId = JsonUtility.FromJson<DeepWallCommonResponseModel>(eventData).pageId;
            _eventListener?.DeepWallPaywallNotOpened(pageId);
        }

        //Paywall action show disabled event.
        public void DeepWallPaywallActionShowDisabled(string eventData)
        {
            int pageId = JsonUtility.FromJson<DeepWallCommonResponseModel>(eventData).pageId;
            _eventListener?.DeepWallPaywallActionShowDisabled(pageId);
        }

        //Paywall closed event
        public void DeepWallPaywallClosed(string eventData)
        {
            int pageId = JsonUtility.FromJson<DeepWallCommonResponseModel>(eventData).pageId;
            _eventListener?.DeepWallPaywallClosed(pageId);
        }

        //Paywall purchasing product event
        public void DeepWallPaywallPurchasingProduct(string eventData)
        {
            _eventListener?.DeepWallPaywallPurchasingProduct(eventData);
        }

        //Purchase success event. Fired after receipt validation if Ploutos service active.
        public void DeepWallPaywallPurchaseSuccess(string eventData)
        {
            var paywallPurchaseSuccess = JsonUtility.FromJson<PaywallPurchaseSuccessModel>(eventData);
            int type = paywallPurchaseSuccess.type;
            string result = paywallPurchaseSuccess.result;
            _eventListener?.DeepWallPaywallPurchaseSuccess(type, result);
        }

        //Purchase failed event
        public void DeepWallPaywallPurchaseFailed(string eventData)
        {
            var paywallPurchaseFailed = JsonUtility.FromJson<DeepWallPaywallPurchaseFailedModel>(eventData);
            int productCode = paywallPurchaseFailed.productCode;
            string reason = paywallPurchaseFailed.reason;
            string errorCode = paywallPurchaseFailed.errorCode;
            bool isPaymentCancelled = paywallPurchaseFailed.isPaymentCancelled;
            _eventListener?.DeepWallPaywallPurchaseFailed(productCode, reason, errorCode, isPaymentCancelled);
        }

        //Restore success event
        public void DeepWallPaywallRestoreSuccess(string eventData)
        {
            _eventListener?.DeepWallPaywallRestoreSuccess();
        }

        //Restore failed event
        public void DeepWallPaywallRestoreFailed(string eventData)
        {
            var paywallRestoreFailed = JsonUtility.FromJson<DeepWallPaywallRestoreFailedModel>(eventData);
            int productCode = paywallRestoreFailed.productCode;
            string reason = paywallRestoreFailed.reason;
            string errorCode = paywallRestoreFailed.errorCode;
            bool isPaymentCancelled = paywallRestoreFailed.isPaymentCancelled;
            _eventListener?.DeepWallPaywallRestoreFailed(productCode, reason, errorCode, isPaymentCancelled);
        }

        //Extra data received event
        public void DeepWallPaywallExtraDataReceived(string eventData)
        {
            _eventListener?.DeepWallPaywallExtraDataReceived(eventData);
        }

        public void DeepWallPaywallConsumeSuccess(string eventData)
        {
            _eventListener?.DeepWallPaywallConsumeSuccess(eventData);
        }

        public void DeepWallPaywallConsumeFail(string eventData)
        {
            _eventListener?.DeepWallPaywallConsumeFail(eventData);
        }

        public void DeepWallATTStatusChanged()
        {
            _eventListener?.DeepWallATTStatusChanged();
        }

        #endregion
    }
}