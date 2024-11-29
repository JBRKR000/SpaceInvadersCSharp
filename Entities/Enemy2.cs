using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Enemy2 : Enemy1
    {
        private Texture2D customBulletTexture;
        private float customShootInterval = 1.5f;
        public override int score => 300; //nadpisanie scorea
		

		public Enemy2(Texture2D enemyTexture, Vector2 position, Texture2D bulletTexture, List<EnemyBullet> bullets, Game1 game)
            : base(enemyTexture, position, bulletTexture, bullets, game)
        {
            this.customBulletTexture = Game.Content.Load<Texture2D>("bullet2");
			this.maxHealth = 100; // Więcej zdrowia niż domyślnie
			this.health = maxHealth;
		}

        public void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastShot >= customShootInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }

		protected override void Shoot()
        {
            bulletSoundInstance.Volume = 0.35f;
            if (bulletSoundInstance.State != SoundState.Playing)
            {
                bulletSoundInstance.Play();
            }


			Vector2 bulletPosition1 = new Vector2(position.X + enemyTexture.Width / 2, position.Y);
			Vector2 bulletPosition2 = new Vector2(position.X + enemyTexture.Width / 2 - 50, position.Y);
			bullets.Add(new EnemyBullet(bulletTexture, bulletPosition1, 20)); //inny damage
			bullets.Add(new EnemyBullet(bulletTexture, bulletPosition2, 20));
		}
    }
}