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
        private Vector2 direction;   // Kierunek lotu w stronę gracza
        private float lifetime;      // Czas życia pocisku
        private const float maxLifetime = 1.5f; // Maksymalny czas życia w sekundach

        public SpiralEnemyBullet(Texture2D bulletTexture, Vector2 startPosition, int damage, float rotationSpeed, float radius)
            : base(bulletTexture, startPosition, damage)
        {
            this.rotationSpeed = rotationSpeed;
            this.radius = radius;
            this.angle = 0f;
            this.lifetime = 0f;
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            Vector2 playerPosition = Player.GetPlayerPos();
            direction = playerPosition - position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            lifetime += deltaTime;
            if (lifetime >= maxLifetime)
            {
                position.X = 10000;
            }
            angle += rotationSpeed * deltaTime/10;
            position.X += (float)(Math.Cos(angle) * radius);
            position += direction * bulletSpeed * deltaTime;
            rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height);
            UpdateDirection();
        }
    }
}
