using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class SpiralEnemyBullet : EnemyBullet
    {
        private float rotationSpeed; // Prędkość obrotu
        private float radius;        // Promień okręgu
        private float angle;         // Aktualny kąt w radianach

        public SpiralEnemyBullet(Texture2D bulletTexture, Vector2 startPosition, int damage, float rotationSpeed, float radius)
            : base(bulletTexture, startPosition, damage)
        {
            this.rotationSpeed = rotationSpeed;
            this.radius = radius;
            this.angle = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            // Oblicz deltaTime
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Aktualizuj kąt
            angle += rotationSpeed * deltaTime;

            // Oblicz ruch w osi X na podstawie kąta i promienia
            position.X += (float)(Math.Cos(angle) * radius);

            // Poruszaj pocisk w dół w osi Y
            position.Y += bulletSpeed * deltaTime;

            // Aktualizacja prostokąta kolizji
            rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height);
        }
    }
}