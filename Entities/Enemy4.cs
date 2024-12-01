﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SpaceInvaders.Entities
{
	internal class Enemy4 : Enemy1
	{
		private Texture2D customBulletTexture;
		private float customShootInterval = 1f;
		public override int score => 50; // Nadpisanie score'a

		public Enemy4(Texture2D enemyTexture, Vector2 position, Texture2D bulletTexture, List<EnemyBullet> bullets, Game1 game)
			: base(enemyTexture, position, bulletTexture, bullets, game)
		{
			this.customBulletTexture = Game.Content.Load<Texture2D>("bulletx");
			this.maxHealth = 150;
			this.health = maxHealth;
			this.enemySpeed = 50f; // Nadpisanie prędkości, aby była szybsza
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			
		}

		protected override void Shoot()
		{
			bulletSoundInstance.Volume = 0.35f;
			if (bulletSoundInstance.State != SoundState.Playing)
			{
				bulletSoundInstance.Play();
			}

			Vector2 bulletPosition1 = new Vector2(position.X + enemyTexture.Width / 2 + 100 , position.Y);
			bullets.Add(new AdvancedEnemyBullet(bulletTexture, bulletPosition1, 10, 5, 5)); // Inny damage
			Vector2 bulletPosition2 = new Vector2(position.X + enemyTexture.Width / 2 , position.Y);
			bullets.Add(new AdvancedEnemyBullet(bulletTexture, bulletPosition2, 10, 5, 5)); // Inny damage
			Vector2 bulletPosition3 = new Vector2(position.X + enemyTexture.Width / 2 -100 , position.Y);
			bullets.Add(new AdvancedEnemyBullet(bulletTexture, bulletPosition3, 10, 5, 5)); // Inny damage
		}
	}
}