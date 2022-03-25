using System.Collections.Generic;
using GameAnalyticsSDK;

namespace HWGames.Bundles.Internal.Analytics {
    internal class GameAnalyticsProvider : IAnalyticsProvider {
        private const string TAG = "GameAnalyticsProvider";

        internal GameAnalyticsProvider() {
            RegisterEvents();
        }

        public void Initialize(bool consent) {
            if (!GameAnalyticsWrapper.Initialize(consent)) {
                UnregisterEvents();
            }
        }

        private static void RegisterEvents() {
            AnalyticsManager.OnGameStartedEvent += OnGameStarted;
            AnalyticsManager.OnGameFinishedEvent += OnGameFinished;
            //AnalyticsManager.OnTrackCustomValueEvent += TrackCustomEvent;
            AnalyticsManager.OnTrackCustomEvent += TrackCustomEvent;
            AnalyticsManager.OnTrackArrayCustomEvent += TrackArrayCustomEvent;
        }

        private static void UnregisterEvents() {
            AnalyticsManager.OnGameStartedEvent -= OnGameStarted;
            AnalyticsManager.OnGameFinishedEvent -= OnGameFinished;
            //AnalyticsManager.OnTrackCustomValueEvent -= TrackCustomEvent;
            AnalyticsManager.OnTrackCustomEvent -= TrackCustomEvent;
            AnalyticsManager.OnTrackArrayCustomEvent -= TrackArrayCustomEvent;
        }

        private static void OnGameStarted(string level, Dictionary<string, object> eventProperties) {
            GameAnalyticsWrapper.TrackProgressEvent(GAProgressionStatus.Start, level, null);
        }

        private static void OnGameFinished(bool levelComplete, float score, string levelNumber, Dictionary<string, object> eventProperties) {
            GameAnalyticsWrapper.TrackProgressEvent(levelComplete ? GAProgressionStatus.Complete : GAProgressionStatus.Fail, levelNumber, (int)score);
        }

        private static void TrackCustomEvent(string eventName,
                                             Dictionary<string, object> eventProperties,
                                             string type,
                                             List<global::SDKBundle.AnalyticsProvider> analyticsProviders) {
            if (analyticsProviders.Contains(global::SDKBundle.AnalyticsProvider.GameAnalytics)) {
                GameAnalyticsWrapper.TrackDesignEvent(eventName, eventProperties);
            }
        }

        private static void TrackArrayCustomEvent(string eventName, object[] eventProperties) {
            GameAnalyticsWrapper.TrackDesignEvent(eventName, eventProperties);
        }
    }
}