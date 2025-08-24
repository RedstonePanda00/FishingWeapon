using UnityEngine;
using Verse;

namespace FishingWeaponMod
{
    // 模块一：设置数据类 (MVC中的Model)
    // 负责存储和加载“特殊鱼出现概率”这个数据
    public class FishingWeaponSettings : ModSettings
    {
        public float specialFishChance = 0.1f; // 默认值为 10%

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref specialFishChance, "specialFishChance", 0.1f);
        }
    }

    // 模块二：设置入口和界面控制类 (MVC中的View & Controller)
    public class FishingWeaponSettingsController : Mod
    {
        // 引用设置数据的实例
        public static FishingWeaponSettings settings;

        public FishingWeaponSettingsController(ModContentPack content) : base(content)
        {
            // 加载设置
            settings = GetSettings<FishingWeaponSettings>();
        }

        // 在Mod设置菜单中显示的名字
        public override string SettingsCategory()
        {
            return "Fishing Weapon"; // 设置页面的名字
        }

        // 绘制设置窗口的具体内容
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("FishingWeapon_SpecialFishChance".Translate() + ": " + settings.specialFishChance.ToStringPercent());
            settings.specialFishChance = listingStandard.Slider(settings.specialFishChance, 0.0f, 1.0f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }
    }
}