using Verse;

namespace FishingWeapon
{
    public class CompProperties_TemperatureWeapon : CompProperties
    {
        public float maxTemp = 25f;

        public float minTemp = 0f;

        public ThingDef targetWeaponDef;

        public float progressPerTickRare = 0.005f;

        public float regressionPerTickRare = 0.0025f;

        public CompProperties_TemperatureWeapon()
        {
            this.compClass = typeof(CompTemperatureWeapon);
        }
    }
}