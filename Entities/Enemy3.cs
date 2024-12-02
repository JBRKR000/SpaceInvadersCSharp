using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SpaceInvaders.Entities
{
	internal class Enemy3 : Enemy1
	{
		private Texture2D customBulletTexture;
		private float customShootInterval = 1f;
		public override int score => 50; // Nadpisanie score'a
		protected SoundEffect bulletSound = SoundEffect.FromFile("../../../Content/Sounds/4.wav");
		protected SoundEffectInstance bulletSoundInstance;
		public Enemy3(Texture2D enemyTexture, Vector2 position, Texture2D bulletTexture, List<EnemyBullet> bullets, Game1 game)
			: base(enemyTexture, position, bulletTexture, bullets, game)
		{
			bulletSoundInstance = bulletSound.CreateInstance();
			this.customBulletTexture = Game.Content.Load<Texture2D>("bullet3");
			this.maxHealth = 20; // Więcej zdrowia niż domyślnie
			this.health = maxHealth;
			this.enemySpeed = 400f; // Nadpisanie prędkości, aby była szybsza
		}

		public override void Update(GameTime gameTime)
		{
			// Zachowujemy działanie metody Update, ale korzystamy z nowej prędkości
			base.Update(gameTime); // Wywołanie metody bazowej
		}

		protected override void Shoot()
		{
			bulletSoundInstance.Volume = 0.35f;
			if (bulletSoundInstance.State != SoundState.Playing)
			{
				bulletSoundInstance.Play();
			}

			Vector2 bulletPosition1 = new Vector2(position.X + enemyTexture.Width / 2 - 50, position.Y);
			bullets.Add(new EnemyBullet(bulletTexture, bulletPosition1, 10)); // Inny damage
		}
	}
}
