using IL.Terraria.Audio;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TestModScholar.Projectiles;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TestModScholar.Items
{
    public class TestStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("TestStaff"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("This is a basic modded staff.");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.mana = 5;
            Item.DamageType = DamageClass.Magic;
            Item.width = 200;
            Item.height = 200;
            Item.useTime = 60;
            Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = 2;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiningProjectile>();
            Item.shootSpeed = 10f;
            Item.noMelee = true;
        }

        int mode;
        int swapCooldown;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (mode == 0 && swapCooldown <= 0)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, player.position);
                    swapCooldown = 120;
                    mode = 1;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Rectangle textpos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
                        CombatText.NewText(textpos, new Color(150, 10, 30, 100), "Fireball", false, false);
                    }
                }
                if (mode == 1 && swapCooldown <= 0)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, player.position);
                    swapCooldown = 120;
                    mode = 2;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Rectangle textpos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
                        CombatText.NewText(textpos, new Color(30, 10, 150, 100), "Spread", false, false);
                    }
                }
                if (mode == 2 && swapCooldown <= 0)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, player.position);
                    swapCooldown = 120;
                    mode = 0;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Rectangle textpos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
                        CombatText.NewText(textpos, new Color(10, 150, 30, 100), "Break", false, false);
                    }
                }
            }
            return base.CanUseItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }

        public override void HoldItem(Player player)
        {
            swapCooldown--;
            if (mode == 0)
            {
                Item.useStyle = 5;
                Item.shootSpeed = 10f;
                Item.useTime = 25;
                Item.noMelee = true;
                Item.autoReuse = true;
            }
            if (mode == 1)
            {
                Item.useStyle = 5;
                Item.shootSpeed = 10f;
                Item.useTime = 45;
                Item.noMelee = true;
                Item.autoReuse = true;
            }
            if (mode == 2)
            {
                Item.useStyle = 5;
                Item.shootSpeed = 10f;
                Item.useTime = 45;
                Item.noMelee = true;
                Item.autoReuse = true;
            }
            base.HoldItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                //Don't use weapon if the player is switching modes
            }
            else
            {
                if (mode == 0)
                {
                    Vector2 offset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;
                    if (Collision.CanHit(position, 0, 0, position + offset, 0, 0))
                    {
                        position += offset;
                    }
                    Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<MiningProjectile>(), 0, knockback, player.whoAmI);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, player.position);
                }
                if (mode == 1)
                {
                    Vector2 offset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;
                    if (Collision.CanHit(position, 0, 0, position + offset, 0, 0))
                    {
                        position += offset;
                    }
                    Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DamageProjectile>(), damage, knockback, player.whoAmI);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, player.position);
                }
                if (mode == 2)
                {
                    Vector2 spreadOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;
                    if (Collision.CanHit(position, 0, 0, position + spreadOffset, 0, 0))
                    {
                        position += spreadOffset;
                    }
                    Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SpreadDamageProjectile>(), damage / 4, knockback, player.whoAmI);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, player.position);

                    int numberProjectiles = 3 + Main.rand.Next(4);
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(15));
                        float scale = 1f - (Main.rand.NextFloat() * 0.3f);
                        perturbedSpeed = perturbedSpeed * scale;
                        Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<SpreadDamageProjectile>(), damage / 4, knockback, player.whoAmI);
                    }
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}