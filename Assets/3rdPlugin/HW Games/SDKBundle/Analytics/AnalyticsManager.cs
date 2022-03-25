using System;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;


namespace HWGames.Bundles.Internal.Analytics {
    internal static class AnalyticsManager {
        private const string TAG = "HWGamesAnalytics";
        private const string NO_GAME_LEVEL = "game";

        internal static bool HasGameStarted { get; private set; }


        //  additional events
        internal static event Action<int, bool> OnGamePlayed;
        internal static event Action<string, Dictionary<string, object>> OnGameStartedEvent;
        internal static event Action<bool, float, string, Dictionary<string, object>> OnGameFinishedEvent;

        internal static event Action<string, float> OnTrackCustomValueEvent;
        internal static event Action OnApplicationFirstLaunchEvent;
        internal static event Action OnApplicationLaunchEvent;
        internal static event Action OnApplicationResumeEvent;

        internal static event Action<string, Dictionary<string, object>, string, List<global::SDKBundle.AnalyticsProvider>> OnTrackCustomEvent;
        internal static event Action<string, object[]> OnTrackArrayCustomEvent;

        private static readonly List<global::SDKBundle.AnalyticsProvider> DefaultAnalyticsProvider = new List<global::SDKBundle.AnalyticsProvider>
            { global::SDKBundle.AnalyticsProvider.GameAnalytics};

        private static GameAnalyticsProvider GameAnalyticsProvider;

        internal static void Initialize(SDKBundleSettings Settings, bool consent) {
            // Initialize providers
            if (Settings.enableGameAnalytics) {
                GameAnalyticsProvider = new GameAnalyticsProvider();
                GameAnalyticsProvider.Initialize(consent);
            }
            //HWAnalyticsProvider = new HWAnalyticsProvider(new HWAnalyticsParameters(false, true, Settings.EditorIdfa));
            //HWAnalyticsProvider.Initialize(consent);
            //Init Facebook
            FB.Init();
            HWLog.Log("HWSDKBundle", "Initializing Facebook");
        }
        internal static void OnApplicationResume() {
            OnApplicationResumeEvent?.Invoke();
        }



        // Track game events
        // ================================================
        internal static void TrackApplicationLaunch() {
            AnalyticsStorageHelper.IncrementAppLaunchCount();
            //fire app launch events
            if (AnalyticsStorageHelper.IsFirstAppLaunch()) {
                OnApplicationFirstLaunchEvent?.Invoke();
            }

            OnApplicationLaunchEvent?.Invoke();
        }


        internal static void TrackGameStarted(string level, Dictionary<string, object> eventProperties = null) {
            HasGameStarted = true;
            AnalyticsStorageHelper.IncrementGameCount();
            OnGameStartedEvent?.Invoke(level ?? NO_GAME_LEVEL, eventProperties);
        }

        internal static void TrackGameFinished(bool levelComplete, float score, string level, Dictionary<string, object> eventProperties) {
            HasGameStarted = false;
            AnalyticsStorageHelper.UpdateLevel(level);
            if (levelComplete) {
                // used to calculate the win rate (for RemoteConfig)
                AnalyticsStorageHelper.IncrementSuccessfulGameCount();
            }

            OnGamePlayed?.Invoke(AnalyticsStorageHelper.GetGameCount(), AnalyticsStorageHelper.UpdateGameHighestScore(score));
            OnGameFinishedEvent?.Invoke(levelComplete, score, level ?? NO_GAME_LEVEL, eventProperties);
        }

        internal static void TrackCustomEvent(string eventName,
                                              Dictionary<string, object> eventProperties,
                                              string type = null,
                                              List<global::SDKBundle.AnalyticsProvider> analyticsProviders = null) {
            if (analyticsProviders == null || analyticsProviders.Count == 0) {
                analyticsProviders = DefaultAnalyticsProvider;
            }

            OnTrackCustomEvent?.Invoke(eventName, eventProperties, type, analyticsProviders);
        }

        internal static void TrackCustomEvent(string eventName, params object[] _objs) {
            OnTrackArrayCustomEvent?.Invoke(eventName, _objs);
        }
    }
}