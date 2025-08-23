using System.Collections.Generic;
using System.Reflection;
using FishingWeapon;
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
    [StaticConstructorOnStartup]
    public static class FishingDefCache
    {
        public static readonly List<ThingDef> ThingDefsWithFishingExtra = new List<ThingDef>();

        static FishingDefCache()
        {
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
            {
                if (def.GetModExtension<FishingExtraThingModextention>() != null)
                {
                    ThingDefsWithFishingExtra.Add(def);
                }
            }
        }
    }
}
