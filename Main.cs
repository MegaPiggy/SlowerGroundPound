using HarmonyLib;
using SALT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SlowerGroundPound
{
    public class Main : ModEntryPoint
    {
        // THE EXECUTING ASSEMBLY
        public static Assembly execAssembly;

        public static float? BaseGP;

        // Called before MainScript.Awake
        // You want to register new things and enum values here, as well as do all your harmony patching
        public override void PreLoad()
        {
            // Gets the Assembly being executed
            execAssembly = Assembly.GetExecutingAssembly();
            HarmonyInstance.PatchAll(execAssembly);
        }


        // Called before MainScript.Start
        // Used for registering things that require a loaded gamecontext
        public override void Load()
        {
            UserInputService.Instance.InputBegan += InputBegan;
        }

        // Called after all mods Load's have been called
        // Used for editing existing assets in the game, not a registry step
        public override void PostLoad()
        {

        }

        public void InputBegan(UserInputService.InputObject inputObject, bool wasProcessed)
        {
            if (inputObject.keyCode == KeyCode.LeftBracket)
                PlayerScript.player.groundPoundSpeed += 10;
            else if (inputObject.keyCode == KeyCode.RightBracket)
                PlayerScript.player.groundPoundSpeed -= 10;
        }

        [HarmonyPatch(typeof(PlayerScript))]
        [HarmonyPatch("Start")]
        private class Patch_GPSpeedChange
        {
            public static void Postfix(PlayerScript __instance)
            {
                if (!BaseGP.HasValue)
                    BaseGP = __instance.groundPoundSpeed;
                __instance.groundPoundSpeed = BaseGP.Value / 3;
            }
        }

    }
}