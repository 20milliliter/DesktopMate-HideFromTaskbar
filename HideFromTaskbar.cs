using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(DesktopMate_HideFromTaskbarMod.HideFromTaskbar), "DesktopMate-HideFromTaskbar", "1.0.1", "20ml", null)]
[assembly: MelonGame("infiniteloop", "DesktopMate")]

namespace DesktopMate_HideFromTaskbarMod
{
    public class HideFromTaskbar : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
        
        private bool awaitingFocus = false;
        private bool wasFocused = false;
        private bool hasHidden = false;
        public override void OnUpdate()
        {
            if (hasHidden) { return; }

            wasFocused = Input.GetMouseButtonDown(0);
            if (wasFocused)
            {
                LoggerInstance.Msg("DesktopMate was focused.");
            }
            if (awaitingFocus && wasFocused) {
                HideActiveWindowFromTaskbar();
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            awaitingFocus = true;
            LoggerInstance.Msg("Waiting for window focus to hide DesktopMate from taskbar.");
        }

        // Code from https://discussions.unity.com/t/can-the-taskbar-icon-of-a-unity-game-be-hidden/790409/3
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        [DllImport("User32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_EXSTYLE = -0x14;
        private const int WS_EX_TOOLWINDOW = 0x0080;

        public void HideActiveWindowFromTaskbar()
        {
            LoggerInstance.Msg("Hiding DesktopMate from taskbar.");
            IntPtr pMainWindow = GetActiveWindow();
            SetWindowLong(pMainWindow, GWL_EXSTYLE, GetWindowLong(pMainWindow, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
            hasHidden = true;
        }
    }
}