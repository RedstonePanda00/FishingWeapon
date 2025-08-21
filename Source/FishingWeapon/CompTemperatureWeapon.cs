using UnityEngine;
using Verse;
using System.Text;
using RimWorld;

namespace FishingWeapon
{
    public class CompTemperatureWeapon : ThingComp
    {
        private float progress = 0f;

        public CompProperties_TemperatureWeapon Props => (CompProperties_TemperatureWeapon)this.props;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref this.progress, "progress", 0f);
        }

        public override void CompTickRare()
        {
            if (!this.parent.Spawned)
            {
                return;
            }

            float ambientTemperature = this.parent.AmbientTemperature;
            
            Log.Message($"[FishingWeapon] 武器: {this.parent.Label}, 当前温度: {ambientTemperature:F1}°C, minTemp配置: {Props.minTemp}°C, 当前进度: {this.progress:P}");

            if (ambientTemperature < Props.minTemp)
            {
                this.progress += Props.progressPerTickRare;

                if (this.progress >= 1f)
                {
                    this.progress = 1f;
                    TransformWeapon();
                }
            }
            else
            {
                if (this.progress > 0)
                {
                    this.progress -= Props.regressionPerTickRare;
                }
            }

            this.progress = Mathf.Clamp01(this.progress);
        }

        public override string CompInspectStringExtra()
        {
            StringBuilder stringBuilder = new StringBuilder();

            float ambientTemperature = this.parent.Spawned ? this.parent.AmbientTemperature : 21f;

            if (ambientTemperature > Props.maxTemp)
            {
                stringBuilder.AppendLine("TemperatureTooHigh".Translate());
            }

            if (this.progress > 0)
            {
                stringBuilder.AppendLine("ChargeProgress".Translate() + ": " + this.progress.ToStringPercent());
            }

            return stringBuilder.ToString().TrimEndNewlines();
        }

        private void TransformWeapon()
        {
            Map map = this.parent.Map;
            IntVec3 position = this.parent.Position;
            int stackCount = this.parent.stackCount;

            if (this.parent.ParentHolder is Pawn_EquipmentTracker equipment)
            {
                Pawn owner = equipment.pawn;
                owner.equipment.TryDropEquipment(this.parent, out ThingWithComps droppedThing, owner.Position, false);
                map = owner.Map;
                position = owner.Position;
            }

            this.parent.Destroy(DestroyMode.Vanish);

            Thing newWeapon = ThingMaker.MakeThing(Props.targetWeaponDef);
            newWeapon.stackCount = stackCount;

            GenPlace.TryPlaceThing(newWeapon, position, map, ThingPlaceMode.Near);

            Find.LetterStack.ReceiveLetter("WeaponTransformedLabel".Translate(), "WeaponTransformedText".Translate(this.parent.Label, newWeapon.Label), LetterDefOf.PositiveEvent, new TargetInfo(position, map));
        }
    }
}