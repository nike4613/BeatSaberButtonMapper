using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BeatSaberKeyboardMapperPlugin.Harmony
{
    public class BeatSaberPatches
    {
        public static void Patch(HarmonyInstance harmony)
        {
            InputGetKeyAllPatch.Patch(harmony);
            VRControllersInputManagerPatches.Patch(harmony);
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

        class VRControllersInputManagerPatches
        {
            public static void Patch(HarmonyInstance harmony)
            {
                var vrcon = typeof(VRControllersInputManager);
                var self = typeof(VRControllersInputManagerPatches);

                var triggerValue = vrcon.GetMethod("TriggerValue", new Type[] { typeof(UnityEngine.XR.XRNode) });
                var horizontalValue = vrcon.GetMethod("HorizontalAxisValue", new Type[] { typeof(UnityEngine.XR.XRNode) });
                var verticalValue = vrcon.GetMethod("VerticalAxisValue", new Type[] { typeof(UnityEngine.XR.XRNode) });

                var transpiler = self.GetMethod("GetAxisTranspiler", BindingFlags.NonPublic | BindingFlags.Static);

                harmony.Patch(triggerValue, null, null, new HarmonyMethod(transpiler));
                harmony.Patch(horizontalValue, null, null, new HarmonyMethod(transpiler));
                harmony.Patch(verticalValue, null, null, new HarmonyMethod(transpiler));
            }

            private static IEnumerable<CodeInstruction> GetAxisTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                MethodInfo toReplace = typeof(Input).GetMethod("GetAxis", new Type[] { typeof(string) });
                MethodInfo replacement =
                    typeof(VRControllersInputManagerPatches).GetMethod("GetAxisSubstitute", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(string) }, new ParameterModifier[] { });

                var codes = new List<CodeInstruction>(instructions);

                foreach (var code in codes)
                {
                    if (code.opcode == OpCodes.Call)
                    {
                        code.operand = replacement;
                    }
                }

                return codes;
            }

            private static float GetAxisSubstitute(string saxis)
            {
                ControllerAxis? axis = null;
                try
                {
                    axis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), saxis);
                }
                catch (ArgumentException)
                { // no matching name
                  // no problem
                }

                float value = Input.GetAxis(saxis);

                if (Settings.Enabled && axis != null)
                {
                    foreach (var binding in Settings.AxisBindings)
                    {
                        if (binding.Axis == axis.Value)
                        {
                            if (binding.OffValue != null && !Input.GetKey(binding.SourceKey))
                                value = binding.OffValue.Value;
                            else if (Input.GetKey(binding.SourceKey))
                                value = binding.OnValue;
                        }
                    }
                }

                return value;
            }
        }
    }
}
