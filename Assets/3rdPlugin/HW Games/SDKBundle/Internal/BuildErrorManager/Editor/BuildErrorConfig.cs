
using System.Collections.Generic;

namespace HWGames.Bundles.Internal.Editor
{
    public static class BuildErrorConfig
    {
        // Individual offsets by error type 
         private const int no = 0;
         private const int ga = 300;

         public enum ErrorID
         {
             NoHWSettings = no,
             SettingsNoFacebookAppID,
             GANoIOSKey = ga, 
             GANoAndroidAndKey,
             NoAdjustToken,
             INVALID_PLATFORM,
             PACKAGE_NAME_ERRROR,
             NOHWPREFAB,
        }

        public static readonly Dictionary<ErrorID, string> ErrorMessageDict = new Dictionary<ErrorID, string>
        {
             {ErrorID.NoHWSettings, "No Settings file found.  Check your path for Assets/Resources/SDKBundle/Settings.asset"},
             {ErrorID.INVALID_PLATFORM, "Invalid Platform please switch to IOS or Android on your Build Settings"},
             {ErrorID.SettingsNoFacebookAppID, "SDKBundle Settings is missing Facebook App Id"},
             {ErrorID.GANoIOSKey, "SDKBundle Settings is missing iOS GameAnalytics keys"},
             {ErrorID.GANoAndroidAndKey, "SDKBundle Settings is missing Android GameAnalytics keys! add 'ignore' in both fields to disable Android analytics"},
             {ErrorID.NoAdjustToken, "SDKBundle Settings is missing Adjust App Token"},
             {ErrorID.PACKAGE_NAME_ERRROR, "Package Name Setting is Error"},
             {ErrorID.NOHWPREFAB, "A HWSDKBundle object not exists in this scene - Check your first scene and create path HWGames/SDKBundle/Create SDKBundle Object"},
         };
    }
 }
