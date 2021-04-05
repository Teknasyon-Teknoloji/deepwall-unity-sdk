using System.Collections;
using System.Collections.Generic;
using DeepWallModule;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DeepWallModule
{
    public class DeepWallUI
    {
        [MenuItem("Window/DeepWall/Dashboard", false, 100)]
        public static void MenuItemDashboard()
        {
            Application.OpenURL(DeepWallInfo.DeepWallDashboardUrl);
        }

        [MenuItem("Window/DeepWall/Documentation/Getting started", false, 101)]
        public static void MenuItemGettingStarted()
        {
            Application.OpenURL(DeepWallInfo.DeepWallDocUrl);
        }

        [MenuItem("Window/DeepWall/About", false, 102)]
        public static void MenuItemAbout()
        {
            string content =
                $"Version {DeepWallInfo.DeepWallVersionName} ({DeepWallInfo.DeepWallVersionCode})\n\n© Copyright 2021 deepwall";
            EditorUtility.DisplayDialog("About DeepWall", content, "OK");
        }
    }
}
#endif