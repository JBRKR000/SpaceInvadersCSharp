﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class SingleBullet
    {
        private Vector2 position;
        private Texture2D bulletTexture;
        private float bulletSpeed = 1000f;

        public SingleBullet(Texture2D bulletTexture, Vector2 startPosition)
        {
            this.position = startPosition;
            this.bulletTexture = bulletTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y -= bulletSpeed * deltaTime;
        }

        public bool IsOffScreen()
        {
            return position.Y < -bulletTexture.Height;
        }
    }
}