using System;
using System.Collections.Generic;
using System.Linq;
using FishingWeapon;
using HarmonyLib;
using RimWorld;
using Unity.Jobs;
using Verse;
using Verse.AI;
namespace FishingWeaponMod
{
    [HarmonyPatch(typeof(JobDriver_Fish), "CompleteFishingToil")]
    public static class FishingCompletePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref Toil __result, JobDriver_Fish __instance)
        {
            var originalAction = __result.initAction;

            __result.initAction = () =>
            {
                if (!Rand.Chance(FishingWeaponSettingsController.settings.specialFishChance) ||
                !TryAddSpecialFish(__instance.pawn, __instance.job.GetTarget(TargetIndex.A).Cell.GetZone(__instance.pawn.Map) as Zone_Fishing)) //从设置中获取概率
                {
                    originalAction?.Invoke();
                }
            };
        }

        private static bool TryAddSpecialFish(Pawn pawn, Zone_Fishing zone_Fishing)
        {
            var mapbiome = pawn.Map.Biome;
            var specialFishDefs = DefDatabase<ThingDef>.AllDefs
                .Where(def => def.HasModExtension<FishingExtraThingModextention>())
                .Where(def => def.GetModExtension<FishingExtraThingModextention>().biomes.Contains(mapbiome) && zone_Fishing.Cells[0].GetWaterBodyType(pawn.Map).ToString() == def.GetModExtension<FishingExtraThingModextention>().isSalt)
                .ToList();
            Log.Warning(mapbiome.defName);
            Log.Warning(zone_Fishing.Cells[0].GetWaterBodyType(pawn.Map).ToString());

            if (specialFishDefs.Count > 0)
            {
                var specialFish = ThingMaker.MakeThing(specialFishDefs.RandomElement());

                // 生成一般以上的随机品质
                    QualityCategory[] validQualities = {
                    QualityCategory.Normal,
                    QualityCategory.Good,
                    QualityCategory.Excellent,
                    QualityCategory.Masterwork,
                    QualityCategory.Legendary
                };
                QualityCategory randomQuality = validQualities.RandomElement();
                specialFish.TryGetComp<CompQuality>()?.SetQuality(randomQuality, null);
                GenPlace.TryPlaceThing(specialFish, pawn.Position, pawn.Map, ThingPlaceMode.Near);
                Messages.Message("获得了特殊鱼：" + specialFish.def.label, MessageTypeDefOf.PositiveEvent);
                return true;
            }
            return false;
        }
    }
}
