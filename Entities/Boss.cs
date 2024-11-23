using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceInvaders;
using System.Collections.Generic;
using System;

internal class Boss : Enemy1
{
	private Texture2D customBulletTexture;
	private Texture2D laserTexture;
	private float customShootInterval = 1.5f;
	public override int score => 1000; // Nadpisanie scorea
	List<Laser> lasers;

	// Zmienna śledząca środek bossa
	private float bossCenterX;

	public Boss(Texture2D enemyTexture, Vector2 position, Texture2D bulletTexture, List<EnemyBullet> bullets, Game1 game, List<Laser> lasers)
		: base(enemyTexture, position, bulletTexture, bullets, game)
	{
		this.customBulletTexture = Game.Content.Load<Texture2D>("bullet2");
		this.laserTexture = Game.Content.Load<Texture2D>("laserTexture");
		this.maxHealth = 2000; // Więcej zdrowia niż domyślnie
		this.health = maxHealth;
		this.lasers = lasers;
	}

	public void Update(GameTime gameTime)
	{
		Random random = new Random();
		double randomDelay = (random.NextDouble() * 10 / 2.5 * 6); // Skrócone opóźnienie
		randomDelay = randomDelay < 2 ? randomDelay : (random.NextDouble() * 10 / 2.5 * 5);
		randomDelay = Math.Round(randomDelay, 2); // Zaokrąglenie do dwóch miejsc dla większej precyzji

		rectangle = new Rectangle((int)position.X, (int)position.Y, enemyTexture.Width, enemyTexture.Height);
		foreach (Laser laser in lasers)
    {
        laser.UpdateTargetCenter(position.X + enemyTexture.Width / 2);
    }
		// Szybsze zmiany kierunku
		timeSinceLastDirectionChange += (float)gameTime.ElapsedGameTime.TotalSeconds;
		float dynamicDirectionChangeInterval = directionChangeInterval * 0.5f; // Częstsze zmiany
		if (timeSinceLastDirectionChange >= dynamicDirectionChangeInterval)
		{
			direction = random.Next(0, 3) == 0 ? direction * -1 : direction;
			if (direction == 0) direction = 1; // Zabezpieczenie przed brakiem ruchu
			timeSinceLastDirectionChange = 0f;
		}

		// Szybszy ruch
		float dynamicSpeedMultiplier = 2f; // Zwiększenie prędkości o 50%
		position.X += direction * enemySpeed * dynamicSpeedMultiplier * (float)gameTime.ElapsedGameTime.TotalSeconds;
		if (position.X < 0) position.X = 0;
		if (position.X > Game.GraphicsDevice.Viewport.Width - enemyTexture.Width)
			position.X = Game.GraphicsDevice.Viewport.Width - enemyTexture.Width;

		// Aktualizujemy środek bossa (pozycja X + połowa szerokości)
		bossCenterX = position.X + enemyTexture.Width / 2;

		// Dynamiczniejsze strzelanie
		timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
		if (timeSinceLastShot >= randomDelay)
		{
			Shoot();
			timeSinceLastShot = 0f;
		}

		// Usuwanie wygasłych laserów
		lasers.RemoveAll(laser => laser.IsExpired());
	}

	protected override void Shoot()
	{
		bulletSoundInstance.Volume = 0.35f;
		if (bulletSoundInstance.State != SoundState.Playing)
		{
			bulletSoundInstance.Play();
		}

		Vector2 bulletPosition1 = new Vector2(position.X + enemyTexture.Width / 2 - 225, position.Y + 155);
		Vector2 bulletPosition2 = new Vector2(position.X + enemyTexture.Width / 2 - 160, position.Y + 150);
		Vector2 bulletPosition3 = new Vector2(position.X + enemyTexture.Width / 2 + 90, position.Y + 150);
		Vector2 bulletPosition4 = new Vector2(position.X + enemyTexture.Width / 2 + 155, position.Y + 155);

		// Dodajemy zwykłe pociski
		bullets.Add(new EnemyBullet(bulletTexture, bulletPosition1, this, 20)); // inny damage
		bullets.Add(new EnemyBullet(bulletTexture, bulletPosition2, this, 20));
		bullets.Add(new EnemyBullet(bulletTexture, bulletPosition3, this, 20));
		bullets.Add(new EnemyBullet(bulletTexture, bulletPosition4, this, 20));

		// Dodanie laserów, które będą się poruszać z aktualnym środkiem bossa
		lasers.Add(new Laser(laserTexture, bulletPosition2, this, 1, position.X + enemyTexture.Width / 2)); // Laser przy bulletPosition2
		lasers.Add(new Laser(laserTexture, bulletPosition3, this, 1, position.X + enemyTexture.Width / 2)); // Laser przy bulletPosition3
	}
}
