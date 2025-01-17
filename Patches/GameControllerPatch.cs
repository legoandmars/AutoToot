﻿/*
    COPYRIGHT NOTICE:
	© 2022 Thomas O'Sullivan - All rights reserved.

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.

	FILE INFORMATION:
	Name: GameControllerPatch.cs
	Project: AutoToot
	Author: Tom
	Created: 15th October 2022
*/

using HarmonyLib;
using UnityEngine;

namespace AutoToot.Patches;

[HarmonyPatch(typeof(GameController), "Update")]
internal class GameControllerUpdatePatch
{
    static bool Prefix(GameController __instance,
        int ___currentnoteindex, float ___currentnotestarty, float ___currentnotestart, float ___currentnoteend, float ___currentnotepshift,
        ref bool ___noteplaying)
    {
        if (!Plugin.IsInGameplay)
            return true;

        if (Input.GetKeyDown(KeyCode.F8))
            Plugin.ToggleActive();
        
        __instance.controllermode = Plugin.IsActive; //Disables user input for us, nice shortcut!!

        if (Plugin.IsActive)
        {
            if (Plugin.Bot == null)
            {
                Plugin.Bot = new Bot(__instance);
            }
            else
            {
                Plugin.Bot.Update(
                    ___currentnoteindex, 
                    ___currentnotestarty, ___currentnotestart, ___currentnoteend, ___currentnotepshift,
                    ref ___noteplaying
                );
            }
        }

        return true;
    }
}