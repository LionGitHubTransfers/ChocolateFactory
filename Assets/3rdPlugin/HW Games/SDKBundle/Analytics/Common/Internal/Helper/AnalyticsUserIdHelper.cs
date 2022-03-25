using System;
using UnityEngine;

namespace HWGames.Bundles.Internal.Analytics
{
    public static class AnalyticsUserIdHelper
    {
        private const string PlayerPrefUserIdentifierKey = "HW_ANALYTICS_USER_IDENTIFIER";

        public static string GetUserId()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefUserIdentifierKey))
            {
                PlayerPrefs.SetString(PlayerPrefUserIdentifierKey, Guid.NewGuid().ToString());
            }
            return PlayerPrefs.GetString(PlayerPrefUserIdentifierKey);
        }
    }
}