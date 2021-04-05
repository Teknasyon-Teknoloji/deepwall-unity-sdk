using UnityEngine;

namespace DeepWallModule
{
    public class DeepWallUtils
    {
        public static AndroidJavaObject GetUnityActivity()
        {
            var unityplayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            return unityplayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
}