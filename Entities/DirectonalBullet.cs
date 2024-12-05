using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
	public class DirectionalBullet : EnemyBullet
	{
		private Vector2 direction; // Kierunek lotu pocisku

		public DirectionalBullet(Texture2D bulletTexture, Vector2 startPosition, int damage, Vector2 direction, float bulletSpeed = 600f)
			: base(bulletTexture, startPosition, damage)
		{
			this.direction = Vector2.Normalize(direction); // Normalizujemy kierunek, by uniknąć problemów z prędkością
			this.bulletSpeed = bulletSpeed; // Ustawiamy prędkość pocisku
		}

		public override void Update(GameTime gameTime)
		{
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			// Przesuwamy pocisk w zadanym kierunku
			position += direction * bulletSpeed * deltaTime;

			// Aktualizujemy prostokąt kolizji
			rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height);
		}
	}
}

