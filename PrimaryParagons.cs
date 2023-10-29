using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using ModHelperData = PrimaryParagons.ModHelperData;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using UnityEngine;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2Cpp;

[assembly: MelonInfo(typeof(PrimaryParagons.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace PrimaryParagons
{
    public class Main : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg(System.ConsoleColor.Cyan, "Primary Paragons Loaded!");
        }
        public class BombParagon
        {
            public class MOABExecutioner : ModVanillaParagon
            {
                public override string BaseTower => "BombShooter-520";
            }
            public class MOABExecutionerUpgrade : ModParagonUpgrade<MOABExecutioner>
            {
                public override int Cost => 900000;
                public override string Description => "Get too close, and you'll be blown to dust.";
                public override string DisplayName => "MOAB Executioner";

                public override string Icon => "MOABExecutioner_Icon";
                public override string Portrait => "MOABExecutioner_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    var attackModel = towerModel.GetAttackModel();
                    var weaponModel = towerModel.GetWeapon();
                    attackModel.weapons[0].Rate = 0.1f;
                    attackModel.weapons[0].projectile.ApplyDisplay<BombShooterParagonDisplay_Projectile>();
                    attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-005").GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].Duplicate());
                    var projectileModel = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
                    projectileModel.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 1000.0f, false, true));
                    projectileModel.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1.0f, 1000.0f, false, true));
                    projectileModel.GetDamageModel().damage = 250.0f;
                    projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    var clusterModel = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile;
                    clusterModel.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
                    clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
                    clusterModel.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1.0f, 100.0f, false, true));
                    clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
                    clusterModel.pierce = 100.0f;
                    clusterModel.maxPierce = 100.0f;
                    clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.pierce = 100.0f;
                    clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.maxPierce = 100.0f; ;
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                    attackModel.attackThroughWalls = true;
                }
            }
            public class MOABExecutionerDisplay : ModTowerDisplay<MOABExecutioner>
            {
                public override string BaseDisplay => GetDisplay(TowerType.BombShooter, 0, 5, 0);
                public override bool UseForTower(int[] tiers)
                {
                    return IsParagon(tiers);
                }

                public override int ParagonDisplayIndex => 0;

                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshTexture(node, "MOABExecutioner_Display");
                }
            }
            public class BombShooterParagonDisplay_Projectile : ModDisplay
            {
                public override string BaseDisplay => "e5edd901992846e409326a506d272633";
                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    foreach (var renderer in node.GetRenderers<MeshRenderer>())
                    {
                        renderer.material.mainTexture = GetTexture("MOABExecutioner_Display");
                    }
                }
            }
        }
        public class GlueParagon
        {
            public class SuperbGlue : ModVanillaParagon
            {
                public override string BaseTower => "GlueGunner-205";
            }
            public class SuperbGlueUpgrade : ModParagonUpgrade<SuperbGlue>
            {
                public override int Cost => 750000;
                public override string Description => "Glue that completely stops almost all Bloons and decimates every type of Bloon. Bloons affected by glue take extra damage.";
                public override string DisplayName => "Superb Glue";

                public override string Icon => "SuperbGlue_Icon";
                public override string Portrait => "SuperbGlue_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    var attackModel = towerModel.GetAttackModel();
                    towerModel.range *= 1.5f;
                    attackModel.range *= 1.5f;
                    attackModel.weapons[0].Rate = 0.2f;
                    attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
                    attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-250").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().Duplicate());
                    attackModel.weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition = 500f;
                    attackModel.weapons[0].projectile.GetDescendants<DamageOverTimeModel>().ForEach(model3 => model3.Interval = 0.05f);
                    attackModel.weapons[0].projectile.GetDescendants<DamageOverTimeModel>().ForEach(model3 => model3.damage = 200f);
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                    attackModel.attackThroughWalls = true;
                }
            }
            public class SuperbGlueDisplay : ModTowerDisplay<SuperbGlue>
            {
                public override string BaseDisplay => GetDisplay(TowerType.GlueGunner, 2, 0, 5);

                public override bool UseForTower(int[] tiers)
                {
                    return IsParagon(tiers);
                }

                public override int ParagonDisplayIndex => 0;

                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshTexture(node, "SuperbGlue_Display");
                }
            }
        }
        public class IceParagon
        {
            public class _0Kelvin : ModVanillaParagon
            {
                public override string BaseTower => "IceMonkey-520";
            }
            public class _0KelvinUpgrade : ModParagonUpgrade<_0Kelvin>
            {
                public override int Cost => 600000;
                public override string Description => "Only the strongest of Bloons are able to resist the cold icy winds.";
                public override string DisplayName => "0° Kelvin";

                public override string Icon => "0Kelvin_Icon";
                public override string Portrait => "0Kelvin_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-042").GetBehavior<SlowBloonsZoneModel>().Duplicate());
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-042").GetBehavior<LinkDisplayScaleToTowerRangeModel>().Duplicate());
                    towerModel.GetBehavior<SlowBloonsZoneModel>().speedScale = 0.5f;
                    towerModel.GetBehavior<SlowBloonsZoneModel>().bindRadiusToTowerRange = false;
                    towerModel.GetBehavior<SlowBloonsZoneModel>().zoneRadius = 9999.0f;
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-042").GetBehaviors<DisplayModel>()[1].Duplicate());
                    towerModel.GetBehavior<LinkDisplayScaleToTowerRangeModel>().baseTowerRange = 9999.0f;
                    towerModel.GetBehavior<LinkDisplayScaleToTowerRangeModel>().displayRadius = 9999.0f;
                    var attackModel = towerModel.GetAttackModel();
                    towerModel.range *= 2.5f;
                    attackModel.range *= 2.5f;
                    attackModel.weapons[0].Rate = 0.05f;
                    attackModel.weapons[0].projectile.GetDamageModel().damage = 200.0f;
                    attackModel.weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition = 50.0f;
                    var iceShard = attackModel.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<EmitOnPopModel>().Duplicate();
                    iceShard.projectile.GetBehavior<TravelStraitModel>().Lifespan = 4.0f;
                    iceShard.projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenEdgeModel_"));
                    attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CPOEFM", iceShard.projectile, iceShard.emission, false, false, false));
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-025").GetAttackModel().Duplicate());
                    var attackModel2 = towerModel.GetAttackModel(1);
                    attackModel2.range = towerModel.range;
                    attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 150.0f;
                    attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    towerModel.GetDescendants<DamageModel>().ForEach(x => x.damage *= 5f);
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                    attackModel.attackThroughWalls = true;
                }
            }
            public class _0KelvinDisplay : ModTowerDisplay<_0Kelvin>
            {
                public override string BaseDisplay => GetDisplay(TowerType.IceMonkey, 5, 2, 0);

                public override bool UseForTower(int[] tiers)
                {
                    return IsParagon(tiers);
                }

                public override int ParagonDisplayIndex => 0;

                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshTexture(node, "0Kelvin_Display");
                }
            }
        }
        public class TackParagon
        {
            public class FieryDoom : ModVanillaParagon
            {
                public override string BaseTower => "TackShooter-205";
            }
            public class FieryDoomUpgrade : ModParagonUpgrade<FieryDoom>
            {
                public override int Cost => 1200000;
                public override string Description => "Flaming tacks and blades so hot that not even purple Bloons are immune.";
                public override string DisplayName => "Fiery Doom";

                public override string Icon => "FieryDoom_Icon";
                public override string Portrait => "FieryDoom_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    var attackModel = towerModel.GetAttackModel();
                    towerModel.range *= 2.0f;
                    attackModel.range *= 2.0f;
                    attackModel.weapons[0].Rate = 0.05f;
                    attackModel.weapons[0].emission.Cast<ArcEmissionModel>().count *= 2;
                    attackModel.weapons[0].projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenEdgeModel_"));
                    var projectileModel = attackModel.weapons[0].projectile;
                    projectileModel.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
                    projectileModel.display.guidRef = "c184360c85b9d70499bb2fff7c77ecb2";
                    projectileModel.pierce = 100.0f;
                    projectileModel.GetDamageModel().damage = 75f;
                    projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-030").GetAttackModel(3).Duplicate());
                    var attackModel2 = towerModel.GetAttackModel(1);
                    attackModel2.range = 2000.0f;
                    attackModel2.fireWithoutTarget = true;
                    attackModel2.weapons[0].Rate = 5.0f;
                    attackModel2.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 250, 0.0f, 360.0f, null, false, false);
                    attackModel2.weapons[0].projectile.pierce = 50.0f;
                    attackModel2.weapons[0].projectile.GetDamageModel().damage = 100.0f;
                    attackModel2.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 100.0f;
                    attackModel2.weapons[0].projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenEdgeModel_"));
                    attackModel2.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("TackShooter-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());
                    var attackModel3 = towerModel.GetAttackModel(2);
                    attackModel3.range = 1000.0f;
                    attackModel3.weapons[0].projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenModel_"));
                    attackModel3.weapons[0].emission.Cast<ArcEmissionModel>().count = 6;
                    attackModel3.weapons[0].projectile.GetDamageModel().damage = 20.0f;
                    attackModel3.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                    attackModel.attackThroughWalls = true;
                }
            }
            public class FieryDoomDisplay : ModTowerDisplay<FieryDoom>
            {
                public override string BaseDisplay => GetDisplay(TowerType.TackShooter, 0, 0, 5);

                public override bool UseForTower(int[] tiers)
                {
                    return IsParagon(tiers);
                }

                public override int ParagonDisplayIndex => 0;

                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshTexture(node, "FieryDoom_Display");
                }
            }
        }
    }
}