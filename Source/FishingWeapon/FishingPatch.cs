using System;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
namespace FishingWeaponMod
{
    [HarmonyPatch(typeof(JobDriver_Fish), "CompleteFishingToil")]
    public static class Patch_CompleteFishingToil
    {
        [HarmonyPrefix]
        public static bool Prefix(out Toil __result, JobDriver_Fish __instance)
        {
            Toil toil = ToilMaker.MakeToil("CustomCompleteFishingToil");
            bool isGotFish=false;
            toil.initAction = () =>
            {
                if (Rand.Chance(1f))
                {
                    Thing extraItem = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("GoldenCube"));
                    GenPlace.TryPlaceThing(extraItem, __instance.pawn.Position, __instance.pawn.Map, ThingPlaceMode.Near);
                    Messages.Message("获得了特殊的鱼："+ extraItem.def.label, MessageTypeDefOf.PositiveEvent);
                }
                else
                {
                    isGotFish= true;
                }

                // 可选：播放声音
                toil.PlaySoundAtStart(SoundDefOf.Interact_CatchFish);

                IntVec3 cell = __instance.job.GetTarget(TargetIndex.A).Cell;
                (cell.GetZone(__instance.pawn.Map) as Zone_Fishing)?.Notify_Fished();
            };

            __result = toil;
            return isGotFish; 
        }
    }
}
