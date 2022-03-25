using System;
using UnityEngine;
using HWGames.Bundles.Internal.Analytics;
using com.adjust.sdk;

namespace HWGames.Bundles.Internal {
    public class HWSDKBundleBehaviour : MonoBehaviour {
        private const string TAG = "HWSDKBundle";
        private static HWSDKBundleBehaviour _instance;
        private SDKBundleSettings _Settings;

        public HWLogLevel HWLogLevel = HWLogLevel.DEBUG;

        private void Awake() {

            if (transform != transform.root)
                throw new Exception("HWSDKBundle prefab HAS to be at the ROOT level!");

            _Settings = SDKBundleSettings.Load();
            if (_Settings == null)
                throw new Exception("Can't find HWSDKBundle Settings file.");

            if (_instance != null) {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
            HWLog.Initialize(HWLogLevel);
            SDKBundle.LionDebuggerHide();
            // init sdk
            if (SDKBundle.IsEnableAdjust()) {
                if (gameObject.GetComponent<Adjust>()==null) {
                    gameObject.AddComponent<Adjust>();
                }
            }
            InitAnalytics();
        }

        private void InitAnalytics() {
            HWLog.Log("HWSDKBundle", "Initializing Analytics");

            AnalyticsManager.Initialize(_Settings, true);

        }

        private void OnApplicationPause(bool pauseStatus) {
            // Brought forward after soft closing 
            // Brought forward after soft closing 
            if (!pauseStatus) {
                AnalyticsManager.OnApplicationResume();
            }
        }
    }
}