using System.Collections.Generic;
using Facebook.Unity.Settings;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using HWGames.Bundles.Internal.Editor;

namespace HWGames.Bundles.Internal.Analytics.Editor
{
    public class FacebookPreBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report)
        {
            CheckAndUpdateFacebookSettings(SDKBundleSettings.Load());
        }

        public static void CheckAndUpdateFacebookSettings(SDKBundleSettings settings)
        {
#if UNITY_ANDROID || UNITY_IOS

      if (settings == null || string.IsNullOrEmpty(settings.facebookAppId))
                BuildErrorWindow.LogBuildError(BuildErrorConfig.ErrorID.SettingsNoFacebookAppID);
            else
            {
                FacebookSettings.AppIds = new List<string> {settings.facebookAppId};
                FacebookSettings.AppLabels = new List<string> {Application.productName};
                EditorUtility.SetDirty(FacebookSettings.Instance);
            }      
#endif
            
        }
    }
}