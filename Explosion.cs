using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Explosion
    {
        private List<Texture2D> frames;
        private Vector2 position;
        private double frameTime;
        private double timeSinceLastFrame;
        private int currentFrame;
        private bool isFinished;

        public bool IsFinished => isFinished;

        public Explosion(List<Texture2D> frames, Vector2 position, double frameTime)
        {
            this.frames = frames;
            this.position = position;
            this.frameTime = frameTime;
            this.timeSinceLastFrame = 0;
            this.currentFrame = 0;
            this.isFinished = false;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrame >= frameTime)
            {
                currentFrame++;
                if (currentFrame >= frames.Count)
                {
                    isFinished = true;
                }
                else
                {
                    timeSinceLastFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentFrame < frames.Count)
            {
                spriteBatch.Draw(frames[currentFrame], position, Color.White);
            }
        }
    }
}