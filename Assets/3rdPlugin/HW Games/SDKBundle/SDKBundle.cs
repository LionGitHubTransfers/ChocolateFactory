#define USE_LION  //导入Lion Analysis后取消注释

using HWGames.Bundles.Internal;
using System.Collections.Generic;
using HWGames.Bundles.Internal.Analytics;
using GameAnalyticsSDK;

#if USE_LION
using LionStudios.Suite.Analytics;
using LionStudios.Suite.Debugging;
#endif

public static class SDKBundle {

    public const string Version = "1.0.2";
    public static string token = "";

    private static SDKBundleSettings _sdkSettings = null;
    public static SDKBundleSettings SDKSettings {
        get {
            if (_sdkSettings == null) {
                _sdkSettings = SDKBundleSettings.Load();
            }
            return _sdkSettings;
        }
    }

#if USE_LION
    // Standard level events
    public static void OnGameStarted() {
        LionAnalytics.GameStart();
    }

    // level = Game Level being played. The first-order of organization in the game
    // attemptNum = number of times user has played Game Level N (second play of Game Level N = 2)
    // score = Game Level score at time of event (if applicable)
    // reward = Reward user received (if applicable)
    public static void LevelStart(int level, int attemptNum, int? score = null) {
        LionAnalytics.LevelStart(level, attemptNum, score);
    }

    public static void LevelSuccess(int level, int attemptNum, int? score = null, Reward reward = null) {
        LionAnalytics.LevelComplete(level, attemptNum, score, reward);
    }

    public static void LevelFailed(int level, int attemptNum, int? score = null) {
        LionAnalytics.LevelFail(level, attemptNum, score);
    }

    public static void LevelRestart(int level, int attemptNum, int? score = null) {
        LionAnalytics.LevelRestart(level, attemptNum, score);
    }

    // Set Player Level (account / user)
    // Optional based on game design
    public static void SetPlayerLevel(int playerLevel) {
        LionAnalytics.SetPlayerLevel(playerLevel);
    }

    public static void ClearPlayerLevel() {
        LionAnalytics.ClearPlayerLevel();
    }

    /// <summary>
    /// Call this method to track any custom event you want.
    /// </summary>
    /// <param name="eventName">事件名</param>
    public static void TrackCustomEvent(string eventName, Dictionary<string, object> eventProperties = null) {
        LionAnalytics.LogEvent(eventName, eventProperties);
        if (eventProperties == null) {
            GameAnalytics.NewDesignEvent("GA_" + eventName);
        }
        else {
            foreach (var item in eventProperties) {
                GameAnalytics.NewDesignEvent("GA_" + eventName + ":" + item.Key + ":" + item.Value);
            }
        }
    }
#endif

    public static string UpdateAdjustToken(SDKBundleSettings settings) {
#if UNITY_IOS
        token = settings.adjustIOSToken;
#endif
#if UNITY_ANDROID
        token = settings.adjustAndroidToken;
#endif

        return token;
    }

    public static void LionDebuggerHide() {
#if USE_LION
        LionDebugger.Hide();
#endif
    }

    public static bool IsEnableGameAnalytics() {
        return SDKSettings.enableGameAnalytics;
    }

    public static bool IsEnableAdjust() {
        return SDKSettings.enableAdjust;
    }

    public static string getToken() {
#if UNITY_ANDROID
        return SDKSettings.adjustAndroidToken.Replace(" ", string.Empty);
#endif
#if UNITY_IOS
        return SDKSettings.adjustIOSToken.Replace(" ", string.Empty);
#endif
        return "";

    }
    public enum AnalyticsProvider {
        HWAnalytics,
        GameAnalytics,
    }
}
