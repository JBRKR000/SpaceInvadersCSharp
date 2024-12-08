using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SpaceInvaders.Entities
{
	internal class Enemy5 : Enemy1
	{
		private Texture2D customBulletTexture;
		private float customShootInterval = 1f;
		public override int score => 500; // Nadpisanie score'a
		protected SoundEffect bulletSound = SoundEffect.FromFile("../../../Content/Sounds/1.wav");
		protected SoundEffectInstance bulletSoundInstance;

		private float teleportInterval = 5f; // Co ile sekund teleportacja
		private float timeSinceLastTeleport = 0f;

		public Enemy5(Texture2D enemyTexture, Vector2 position, Texture2D bulletTexture, List<EnemyBullet> bullets, Game1 game)
			: base(enemyTexture, position, bulletTexture, bullets, game)
		{
			bulletSoundInstance = bulletSound.CreateInstance();
			this.customBulletTexture = Game.Content.Load<Texture2D>("bullet5");
			this.maxHealth = 250;
			this.health = maxHealth;
			this.enemySpeed = 50f; // Nadpisanie prędkości
		}

		public override void Update(GameTime gameTime)
		{
			// Aktualizuj czas od ostatniej teleportacji
			timeSinceLastTeleport += (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Jeśli minęło 5 sekund, teleportuj przeciwnika
			if (timeSinceLastTeleport >= teleportInterval)
			{
				Teleport();
				timeSinceLastTeleport = 0f;
			}

			// Wywołaj oryginalną metodę Update z klasy bazowej
			base.Update(gameTime);
		}

		private void Teleport()
		{
			Random random = new Random();
			int xMargin = 100; // Odstęp od bocznych krawędzi
			int yMargin = 100; // Odstęp od górnych i dolnych krawędzi

			// Losowa pozycja w granicach planszy
			position = new Vector2(
				random.Next(xMargin, Game.GraphicsDevice.Viewport.Width - enemyTexture.Width - xMargin),
				random.Next(yMargin, Game.GraphicsDevice.Viewport.Height / 2 - enemyTexture.Height - yMargin)
			);

		}

		protected override void Shoot()
		{
			bulletSoundInstance.Volume = 0.35f;
			if (bulletSoundInstance.State != SoundState.Playing)
			{
				bulletSoundInstance.Play();
			}

			// Strzelanie w 8 kierunkach
			List<Vector2> directions = new List<Vector2>
			{
				new Vector2(0, -1),   // Góra
				new Vector2(0, 1),    // Dół
				new Vector2(-1, 0),   // Lewo
				new Vector2(1, 0),    // Prawo
				new Vector2(-1, -1),  // Lewo-góra
				new Vector2(1, -1),   // Prawo-góra
				new Vector2(-1, 1),   // Lewo-dół
				new Vector2(1, 1)     // Prawo-dół
			};

			foreach (var direction in directions)
			{
				Vector2 bulletPosition = new Vector2(position.X + enemyTexture.Width / 2 - 50, position.Y + enemyTexture.Height / 2);
				bullets.Add(new DirectionalBullet(customBulletTexture, bulletPosition, 8, direction)); // Tworzymy DirectionalBullet
			}
		}


	}
}
