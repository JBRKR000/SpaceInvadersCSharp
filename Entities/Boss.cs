using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class Boss
    {
        protected Texture2D bossTexture;
        public Vector2 position;
        protected float bossSpeed;
        public Rectangle Rectangle
        {
            get
            {
                // Używamy tekstury do określenia szerokości i wysokości
                return new Rectangle((int)position.X, (int)position.Y, bossTexture.Width, bossTexture.Height);
            }
            set
            {
                // Ustawiamy pozycję (X, Y) i szerokość/ wysokość na podstawie przekazanego Rectangle
                position = new Vector2(value.X, value.Y);
                // Możesz zaktualizować szerokość i wysokość, ale ponieważ są one zależne od tekstury,
                // musisz pamiętać, że nie będziesz zmieniać szerokości / wysokości w tym miejscu, tylko pozycję.
            }
        }
        public int width => bossTexture.Width;
        public int height => bossTexture.Height;
        protected Game1 Game;
        public int health;
        protected double directionChangeInterval;
        protected float timeSinceLastDirectionChange;
        protected Random random;
        protected int directionX;
        protected int directionY;
        protected int maxHealth;
        protected Color healthBarColor = Color.Green;
        protected Color healthBarBackgroundColor = Color.Red;

        private SoundEffect bulletSound;
        private SoundEffectInstance bulletSoundInstance;

        private Texture2D bulletTexture;
        protected List<EnemyBullet> bullets;
        protected double timeSinceLastShot;
        public int score;

        private double stopDuration; // Time for stopping boss movement
        private double stopTimer;

        // Constructor
        public Boss(Texture2D texture, Vector2 position, float bossSpeed, Game1 game, List<EnemyBullet> bullets, Texture2D bulletTexture, int score)
        {
            this.bossTexture = texture;
            this.Game = game;
            this.position = position;
            this.bossSpeed = bossSpeed;
            this.maxHealth = 2000;
            this.health = maxHealth;
            this.bullets = bullets;
            this.bulletTexture = bulletTexture;
            this.score = score;

            bulletSound = SoundEffect.FromFile("../../../Content/Sounds/2.wav");
            bulletSoundInstance = bulletSound.CreateInstance();

            random = new Random();
            directionChangeInterval = random.NextDouble();
            timeSinceLastDirectionChange = 0f;

            stopDuration = 3; // Boss stops for 3 seconds
            stopTimer = 0;
        }

        // Update method
        public void Update(GameTime gameTime)
        {
            Rectangle = new Rectangle((int)position.X, (int)position.Y, bossTexture.Width, bossTexture.Height);
            timeSinceLastDirectionChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate a randomized delay for shooting
            double randomDelay = Math.Round(random.NextDouble() * 10 / 2.5 * 12);
            if (randomDelay < 3) randomDelay = Math.Round(random.NextDouble() * 10 / 2.5 * 10);

            // Change direction at random intervals
            if (timeSinceLastDirectionChange >= directionChangeInterval)
            {
                directionX = random.Next(-1, 2);
                directionY = random.Next(-1, 2);
                timeSinceLastDirectionChange = 0f;
            }

            // Update position based on direction
            position.X += directionX * bossSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += directionY * bossSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Keep the boss within screen boundaries
            position.X = Math.Clamp(position.X, 0, Game.GraphicsDevice.Viewport.Width - bossTexture.Width);
            position.Y = Math.Clamp(position.Y, 0, Game.GraphicsDevice.Viewport.Height / 2 - bossTexture.Height);

            // Handle shooting
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastShot >= randomDelay)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }

        // Draw the boss and its health bar
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bossTexture, position, Color.White);
            DrawHealthBar(spriteBatch);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
            int barWidth = 50;
            int barHeight = 5;
            int healthBarOffset = 10;

            // Calculate health percentage
            float healthPercentage = (float)health / maxHealth;
            int healthBarCurrentWidth = Math.Max(1, (int)(barWidth * healthPercentage)); // Avoid zero width

            // Position health bar below the boss
            Vector2 barPosition = new Vector2(position.X + (bossTexture.Width - barWidth) / 2, position.Y + bossTexture.Height + healthBarOffset);

            // Draw health bar background
            spriteBatch.Draw(Game1.CreateRectangleTexture(Game.GraphicsDevice, barWidth, barHeight, healthBarBackgroundColor), barPosition, Color.White);

            // Draw current health
            spriteBatch.Draw(Game1.CreateRectangleTexture(Game.GraphicsDevice, healthBarCurrentWidth, barHeight, healthBarColor), barPosition, Color.White);
        }

        // Shoot bullets
        protected virtual void Shoot()
        {
            bulletSoundInstance.Volume = 0.25f;
            if (bulletSoundInstance.State != SoundState.Playing)
            {
                bulletSoundInstance.Play();
            }

            // Calculate positions for multiple bullets
            bullets.Add(new EnemyBullet(bulletTexture, new Vector2(position.X + bossTexture.Width / 2 - 227, position.Y + 110), 20));
            bullets.Add(new EnemyBullet(bulletTexture, new Vector2(position.X + bossTexture.Width / 2 - 157, position.Y + 110), 20));
            bullets.Add(new EnemyBullet(bulletTexture, new Vector2(position.X + bossTexture.Width / 2 + 90, position.Y + 110), 20));
            bullets.Add(new EnemyBullet(bulletTexture, new Vector2(position.X + bossTexture.Width / 2 + 157, position.Y + 110), 20));
        }
    }
}
