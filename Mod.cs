using System.Reflection;
using HarmonyLib;
using Verse;

namespace FishingWeaponMod
{
    [StaticConstructorOnStartup]
    public static class FishingWeaponMod
    {
        static FishingWeaponMod()
        {
            var harmony = new Harmony("fish.fishingweaponmod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
