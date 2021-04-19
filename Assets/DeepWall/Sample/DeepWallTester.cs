using DeepWallModule;
using UnityEngine;
using UnityEngine.UI;

namespace DeepWallModule
{
    public class DeepWallTester : MonoBehaviour, IDeepWallEventListener
    {
        [SerializeField] private Text testText;
        private const string AndroidApiKey = "ADD_YOUR_ANDROID_API_KEY_HERE";
        private const string IosApiKey = "ADD_YOUR_IOS_API_KEY_HERE";

        const string ToastClassName = "android.widget.Toast";

        private DeepWallUserProperty _myUserProperty;

        public void InitDeepWallClicked()
        {
            DeepWall.InitDeepWall(AndroidApiKey, IosApiKey,
                DeepWallEnvironment.Sandbox, this);
            testText.text += "\nPlugin activity built";
        }

        public void SetUserPropertiesClicked()
        {
            string myUUID = "deepwalluuidtest";
            _myUserProperty = new DeepWallUserProperty(myUUID, "us", "en-US", DeepWallEnvironmentStyle.DARK);

            DeepWall.SetUserProperties(_myUserProperty);

            testText.text += "\nSuccessfully called setUserProperties";
        }

        public void RequestPaywallClicked()
        {
            string actionKey = "Settings";
            DeepWall.RequestPaywall(actionKey, null);

            /* You can send extra parameter if needed as below:
            DeepWall.RequestPaywall(actionKey, "{\"sliderIndex\": 2, \"title\": \"Deepwall\"}"); 
            */

            testText.text += "\nSuccessfully called requestPaywall";
        }
        
        public void RequestAppTrackingClicked()
        {
            string actionKey = "Settings";
            DeepWall.RequestAppTracking(actionKey, null);

            /* You can send extra parameter if needed as below:
            DeepWall.RequestAppTrackingClicked(actionKey, "{\"sliderIndex\": 2, \"title\": \"Deepwall\"}"); 
            */

            testText.text += "\nSuccessfully called RequestAppTracking";
        }

        public void UpdateUserpropertiesClicked()
        {
            _myUserProperty.Language = "tr-Tr";
            _myUserProperty.Country = "tr";
            _myUserProperty.EnvironmentStyle = DeepWallEnvironmentStyle.LIGHT;

            DeepWall.UpdateUserProperties(_myUserProperty);
            testText.text += "\nSuccessfully called updateUserProperties";
        }

        void UpdateStatusText(string m)
        {
            testText.text += "\nDeepWallEvent: " + m;
            Debug.Log("DeepWallEvent: " + m);
            ShowToast(m);
        }

        private void ShowToast(string text)
        {
#if UNITY_ANDROID
        var toastJavaClass = new AndroidJavaClass(ToastClassName);
        var javaToastObject = toastJavaClass.CallStatic<AndroidJavaObject>("makeText", DeepWallUtils.GetUnityActivity(), text, 1);
        javaToastObject.Call("show");
#elif UNITY_IOS
            DeepWall.ShowAlertMessage(text);
#endif
        }

        public void OnDeepWallPaywallRequested()
        {
            UpdateStatusText("OnDeepWallPaywallRequested");
        }

        public void DeepWallPaywallResponseReceived()
        {
            UpdateStatusText("DeepWallPaywallResponseReceived");
        }

        public void DeepWallPaywallOpened(int pageId)
        {
            UpdateStatusText($"DeepWallPaywallOpened pageId:{pageId}");
        }

        public void DeepWallPaywallNotOpened(int pageId)
        {
            UpdateStatusText($"DeepWallPaywallNotOpened pageId:{pageId}");
        }

        public void DeepWallPaywallActionShowDisabled(int pageId)
        {
            UpdateStatusText($"DeepWallPaywallActionShowDisabled pageId:{pageId}");
        }

        public void DeepWallPaywallClosed(int pageId)
        {
            UpdateStatusText($"DeepWallPaywallClosed pageId:{pageId}");
        }

        public void DeepWallPaywallPurchasingProduct(string productCode)
        {
            UpdateStatusText($"DeepWallPaywallPurchasingProduct productCode:{productCode}");
        }

        public void DeepWallPaywallResponseFailure(string errorCode, string reason)
        {
            UpdateStatusText($"DeepWallPaywallResponseFailure errorCode:{errorCode}, reason: {reason}");
        }

        public void DeepWallPaywallPurchaseSuccess(int type, string result)
        {
            UpdateStatusText($"DeepWallPaywallPurchaseSuccess type:{type}, result: {result}");
        }

        public void DeepWallPaywallPurchaseFailed(int productCode, string reason, string errorCode,
            bool isPaymentCancelled)
        {
            UpdateStatusText(
                $"DeepWallPaywallPurchaseFailed productCode:{productCode}, reason: {reason}, errorCode:{errorCode}, isPaymentCancelled: {isPaymentCancelled}");
        }

        public void DeepWallPaywallRestoreSuccess()
        {
            UpdateStatusText($"DeepWallPaywallRestoreSuccess");
        }

        public void DeepWallPaywallRestoreFailed(int productCode, string reason, string errorCode,
            bool isPaymentCancelled)
        {
            UpdateStatusText(
                $"DeepWallPaywallRestoreFailed productCode:{productCode}, reason: {reason}, errorCode:{errorCode}, isPaymentCancelled: {isPaymentCancelled}");
        }

        public void DeepWallPaywallExtraDataReceived(string data)
        {
            UpdateStatusText($"DeepWallPaywallExtraDataReceived data:{data}");
        }

        public void DeepWallPaywallConsumeSuccess(string data)
        {
            UpdateStatusText($"DeepWallPaywallConsumeSuccess data:{data}");
        }

        public void DeepWallPaywallConsumeFail(string data)
        {
            UpdateStatusText($"DeepWallPaywallConsumeFail data:{data}");
        }

        public void DeepWallATTStatusChanged()
        {
            UpdateStatusText($"DeepWallATTStatusChanged");
        }
    }
}