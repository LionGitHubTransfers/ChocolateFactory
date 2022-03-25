using System.Collections.Generic;
using Facebook.Unity.Settings;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using HWGames.Bundles.Internal.Editor;

namespace HWGames.Bundles.Internal.Analytics.Editor {
    public class AdjustBuildPrebuild : IPreprocessBuildWithReport {
        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report) {
            CheckAndUpdateAdjustSettings(SDKBundleSettings.Load());
        }

        public static void CheckAndUpdateAdjustSettings(SDKBundleSettings Settings) {
            if (!Settings.enableAdjust) {
                return;
            }
#if UNITY_IOS

            if (Settings == null || string.IsNullOrEmpty(Settings.adjustIOSToken.Replace(" ", string.Empty)))
                BuildErrorWindow.LogBuildError(BuildErrorConfig.ErrorID.NoAdjustToken);
#endif
#if UNITY_ANDROID
            if (Settings == null || string.IsNullOrEmpty(Settings.adjustAndroidToken.Replace(" ", string.Empty)))
                BuildErrorWindow.LogBuildError(BuildErrorConfig.ErrorID.NoAdjustToken);
#endif

        }
    }
}