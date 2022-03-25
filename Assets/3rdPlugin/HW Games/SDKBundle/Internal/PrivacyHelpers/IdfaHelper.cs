using UnityEngine;
using HWGames.Bundles.Internal.IdfaAuthorization;

namespace HWGames.Bundles.Internal.PrivacyHelpers
{
    public static class IdfaHelper
    {
        public const string LimitedAdTrackingId = "00000000-0000-0000-0000-000000000000";

        public delegate void RequestAdvertisingIdCallback(string advertisingId, bool trackingEnabled, string error);

        public static void RequestAdvertisingId(RequestAdvertisingIdCallback callBack,
                                                bool forceZerosForLimitedAdTrackingIos,
                                                bool forceZerosForLimitedAdTrackingAndroid)
        {
#if UNITY_EDITOR
            var editorIdfa = SDKBundleSettings.Load().EditorIdfa;
            var finalIdfa = string.IsNullOrEmpty(editorIdfa) ? LimitedAdTrackingId : editorIdfa;
            callBack(finalIdfa, true, null);

#elif UNITY_ANDROID
            Application.RequestAdvertisingIdentifierAsync((advertisingId, adTrackingEnabled, error) => {
               var finalIdfa = (forceZerosForLimitedAdTrackingAndroid && !adTrackingEnabled) ? LimitedAdTrackingId : advertisingId; 
               callBack(finalIdfa, adTrackingEnabled, error);
            });
#elif UNITY_IOS
            
            bool adTrackingEnabled =  NativeWrapper.GetAuthorizationStatus() == IdfaAuthorizationStatus.Authorized;
            var finalIdfa = (forceZerosForLimitedAdTrackingIos && !adTrackingEnabled) ? LimitedAdTrackingId : UnityEngine.iOS.Device.advertisingIdentifier;
            callBack(finalIdfa, adTrackingEnabled, "");
#endif
        }
    }
}
