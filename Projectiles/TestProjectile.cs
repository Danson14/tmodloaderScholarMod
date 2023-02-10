using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;
using Terraria.GameContent.Achievements;
using System.Security.Cryptography;

namespace TestModScholar.Projectiles
{
    public class TestProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }


        public override void Kill(int timeLeft)
        {
            Vector2 position = Projectile.Center;
            int radius = 1;

            Player player = Main.player[Projectile.owner];
            Item bestPickaxe = player.GetBestPickaxe();

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X / 16f);
                    int yPosition = (int)(y + position.Y / 16f);

                    if (xPosition < 0 || xPosition >= Main.maxTilesX || yPosition < 0 || yPosition >= Main.maxTilesY)
                        continue;

                    Tile tile = Main.tile[xPosition, yPosition];

                    if ((x * y) <= radius)
                    {
                        if (Projectile.owner == Main.myPlayer)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                player.PickTile(xPosition, yPosition, bestPickaxe != null ? bestPickaxe.pick : 35);
                            }
                        }
                    }
                }
            }
        }


        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.FrostStaff, 0f, 0f, 0, default(Color), 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.013f;

            int dust2 = Dust.NewDust(Projectile.Center, 1, 1, DustID.Shadowflame, 0f, 0f, 0, default(Color), 1f);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0.3f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.013f;

        }
    }
}
