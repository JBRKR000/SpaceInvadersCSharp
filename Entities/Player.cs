using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{
	public class Player //41x51 takie są jego rozmiary (po teksturze)
	{
		private Texture2D playerTexture;
		private float playerSpeed = 600f;

		private Texture2D bulletTexture;
		private List<SingleBullet> bullets;
		private float shootInterval = 0.25f;
		private float timeSinceLastShot = 0f;
		private bool isGodModeActive = false;
		public static Vector2 playerPos;
		

		public int health = 100;

		public Rectangle rectangle; // ciało gracza

		private SoundEffect bulletSound = SoundEffect.FromFile("../../../Content/Sounds/shoot_player.wav");
		private SoundEffectInstance bulletSoundInstance;

		private bool isFireRateBoosted = false; // Czy boost jest aktywny
		private float fireRateBoostTimer = 0f; // Licznik czasu boosta
		private float normalShootInterval = 0.25f; // Domyślny interwał strzału

		private bool isShieldActive = false;
		private float shieldDuration = 5f; // Czas trwania tarczy w sekundach
		private float shieldTimer = 0f;

		// Dodajemy publiczną właściwość Position
		public Vector2 Position { get; private set; }

		public bool IsShieldActive => isShieldActive;

		// Konstruktor
		public Player(Texture2D playerTexture, Vector2 startPosition, Texture2D bulletTexture, List<SingleBullet> bullets, int health)
		{
			this.playerTexture = playerTexture;
			this.Position = startPosition; // Inicjalizujemy Position
			this.bulletTexture = bulletTexture;
			this.bullets = bullets;
			this.health = health;
			bulletSoundInstance = bulletSound.CreateInstance();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (health > 0)
			{
				spriteBatch.Draw(playerTexture, Position, Color.White);
			}
		}

		public void Update(GameTime gameTime)
		{
			Vector2 magnitude = Vector2.Zero;
			rectangle = new Rectangle((int)Position.X, (int)Position.Y, playerTexture.Width, playerTexture.Height);

			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			var keyboardState = Keyboard.GetState();
			if (keyboardState.IsKeyDown(Keys.G))
			{
				isGodModeActive = !isGodModeActive;
				if (isGodModeActive)
				{
					isShieldActive = true;
					ActivateShield(100000);
				}
				else
				{
					isShieldActive = false;
					ActivateShield(0);
				}
			}
			// Aktualizacja czasu boosta
			if (isFireRateBoosted)
			{
				fireRateBoostTimer -= deltaTime;
				if (fireRateBoostTimer <= 0)
				{
					isFireRateBoosted = false;
					shootInterval = normalShootInterval; // Przywrócenie normalnej wartości interwału strzału
				}
			}

			if (isShieldActive)
			{
				shieldTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (shieldTimer <= 0)
				{
					isShieldActive = false;
				}
			}

			timeSinceLastShot += deltaTime;
			if (health > 0)
			{
				// Ruch gracza
				if (keyboardState.IsKeyDown(Keys.A) && Position.X > 0)
				{
					magnitude.X -= 1;
				}

				if (keyboardState.IsKeyDown(Keys.D) && Position.X < Game1.getScreenWidth() - 50)
				{
					magnitude.X += 1;
				}

				if (keyboardState.IsKeyDown(Keys.W) && Position.Y > Game1.getScreenHeight() / 2)
				{
					magnitude.Y -= 1;
				}

				if (keyboardState.IsKeyDown(Keys.S) && Position.Y < Game1.getScreenHeight() - 50)
				{
					magnitude.Y += 1;
				}

				if (magnitude != Vector2.Zero)
				{
					magnitude.Normalize();
				}
				Position += magnitude * playerSpeed * deltaTime;

				// Strzelanie
				if (keyboardState.IsKeyDown(Keys.Space) && timeSinceLastShot >= shootInterval)
				{
					Shoot();
					timeSinceLastShot = 0f;
				}
			}

			playerPos = Position;
		}

		private void Shoot()
		{
			bulletSoundInstance.Volume = 0.25f;
			bulletSoundInstance.Pitch = .75f;
			bulletSoundInstance.Play();

			Vector2 bulletPosition1 = new Vector2(Position.X + playerTexture.Width / 2, Position.Y);
			Vector2 bulletPosition2 = new Vector2(Position.X + playerTexture.Width / 2 - 40, Position.Y);
			bullets.Add(new SingleBullet(bulletTexture, bulletPosition1, 10)); // Każdy bullet ma swój dmg
			bullets.Add(new SingleBullet(bulletTexture, bulletPosition2, 10));
		}

		public void ActivateFireRateBoost(float duration)
		{
			isFireRateBoosted = true;
			fireRateBoostTimer = duration;
			shootInterval = 0.1f; // Zwiększona szybkostrzelność
		}

		public void ActivateShield(float duration)
		{
			isShieldActive = true;
			shieldTimer = duration;
		}

		public static Vector2 GetPlayerPos()
		{
			return playerPos;
		}
		
	}
}
