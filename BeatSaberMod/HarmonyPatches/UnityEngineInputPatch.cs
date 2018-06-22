using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BeatSaberMod.HarmonyPatches
{
    class UnityEngineInputPatch
    {
        public static void Patch(HarmonyInstance harmony)
        {
            InputGetKeyAllPatch.Patch(harmony);
        }
        
        //[HarmonyPatch(typeof(Input), "GetKeyDown", new Type[] { typeof(KeyCode) })]
        class InputGetKeyAllPatch
        {
            public static void Patch(HarmonyInstance harmony)
            {
                var input = typeof(Input);
                var self = typeof(InputGetKeyAllPatch);

                var getKey = input.GetMethod("GetKey", new Type[] { typeof(KeyCode) });
                var getKeyUp = input.GetMethod("GetKeyUp", new Type[] { typeof(KeyCode) });
                var getKeyDown = input.GetMethod("GetKeyDown", new Type[] { typeof(KeyCode) });

                var getKeyAllPost = self.GetMethod("GetKeyAllPost");

                harmony.Patch(getKey, null, new HarmonyMethod(getKeyAllPost));
                harmony.Patch(getKeyUp, null, new HarmonyMethod(getKeyAllPost));
                harmony.Patch(getKeyDown, null, new HarmonyMethod(getKeyAllPost));
            }

            public static void GetKeyAllPost(ref bool __result, MethodBase __originalMethod, KeyCode key)
            { 
                // it would return true anyway, the plugin isn't enabled, or the input mode doesn't include default
                if (__result || !Settings.Enabled || (Settings.InputMode & InputMode.Default) == 0) return;
                try
                {
                    foreach (var binding in Settings.Bindings)
                    {
                        if (binding.DestKey == key)
                        {
                            __result = __result || (bool)__originalMethod.Invoke(null, new object[] { binding.SourceKey });
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    // do nothing
                }
            }
        }
    }
}
