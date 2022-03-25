using UnityEngine;
using UnityEngine.Serialization;
using System;

using HWGames.Bundles.Internal;

namespace HWGames.Bundles.Internal {
    [SerializeField]
    [CreateAssetMenu(fileName = "Assets/Resources/SDKBundle/SDKBundleSettings", menuName = "HW Games/SDK Bundle")]
    public class SDKBundleSettings : ScriptableObject {
        private const string SETTING_RESOURCES_PATH = "SDKBundle/Settings";

        public static SDKBundleSettings Load() => Resources.Load<SDKBundleSettings>(SETTING_RESOURCES_PATH);

        public bool enableGameAnalytics;
        public string gameAnalyticsAndroidGameKey = "";
        public string gameAnalyticsAndroidSecretKey = "";
        public string gameAnalyticsIosGameKey = "";
        public string gameAnalyticsIosSecretKey = "";

        public string facebookAppId = "";

        public bool enableAdjust;
        public string adjustIOSToken = "";
        public string adjustAndroidToken = "";

        [ReadOnly]
        public string EditorIdfa;

    }
}