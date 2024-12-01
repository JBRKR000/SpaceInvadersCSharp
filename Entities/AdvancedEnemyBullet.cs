using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class AdvancedEnemyBullet : EnemyBullet
    {
        private float oscillationFrequency;
        private float amplitude;
        private float timeElapsed;

        public AdvancedEnemyBullet(Texture2D bulletTexture, Vector2 startPosition, int damage, float oscillationFrequency, float amplitude)
            : base(bulletTexture, startPosition, damage)
        {
            this.oscillationFrequency = oscillationFrequency;
            this.amplitude = amplitude;
            this.timeElapsed = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Dodanie niestandardowej logiki oscylacji
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeElapsed += deltaTime;
            // Oscylacyjny ruch po osi X
            float oscillation = (float)Math.Sin(timeElapsed * oscillationFrequency) * amplitude;
            position.X += oscillation;
            // Aktualizacja prostokąta kolizji
            rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height);
        }
    }
}