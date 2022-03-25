using System.IO;
using UnityEditor.Android;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace HWGames.Bundles.Internal.Editor
{
    public class AndroidPostBuild : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report) {
            
        }

        public void OnPostGenerateGradleAndroidProject(string projectPath)
        {
#if UNITY_2019_3_OR_NEWER
            projectPath += "/../";
#endif
            var fileInfo = new FileInfo(Path.Combine(projectPath, "gradle.properties"));
            string[] content = { "unityStreamingAssets=.unity3d", "android.useAndroidX=true", "android.enableJetifier = true" ,"android.bundle.enableUncompressedNativeLibs = false"};
            File.WriteAllLines(fileInfo.FullName, content);
        }
    }
}

