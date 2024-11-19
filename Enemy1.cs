using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace SpaceInvaders
{
    public class Enemy1
    {
        private Vector2 position;
        private Texture2D enemyTexture;
        private float enemySpeed = 100f;
        public Rectangle rectangle;

        private Texture2D bulletTexture;
        private float shootInterval = 2f;
        private double timeSinceLastShot = 0f;
        private List<EnemyBullet> bullets;
        private Game1 Game;

        private Random random;
        private int direction = 1;
        private float directionChangeInterval = 3f;
        private float timeSinceLastDirectionChange = 0f;

        public int health = 50;
        private int maxHealth = 50;

        private SoundEffect bulletSound = SoundEffect.FromFile("../../../Content/Sounds/2.wav");
        private SoundEffectInstance bulletSoundInstance;
        
        private Color healthBarColor = Color.Green;
        private Color healthBarBackgroundColor = Color.Red;

        public Enemy1(Texture2D enemyTexture, Vector2 position, Texture2D bulletTexture, List<EnemyBullet> bullets, Game1 game)
        {
            this.enemyTexture = enemyTexture;
            this.position = position;
            this.bulletTexture = bulletTexture;
            this.bullets = bullets;
            this.Game = game;
            random = new Random();
            bulletSoundInstance = bulletSound.CreateInstance();
        }

        public void Update(GameTime gameTime)
        {
            Random random = new Random();
            double randomDelay = (random.NextDouble() * 10 / 2.5 * 12);
            randomDelay = randomDelay < 3 ? randomDelay : (random.NextDouble() * 10 / 2.5 * 10);
            double.Round(randomDelay);
            Console.WriteLine("**DEBUG** " + randomDelay + " VALUE");
            
            Console.WriteLine(enemySpeed);
            rectangle = new Rectangle((int)position.X, (int)position.Y, enemyTexture.Width, enemyTexture.Height);

            timeSinceLastDirectionChange += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastDirectionChange >= directionChangeInterval)
            {
                direction = random.Next(0, 2) == 0 ? -1 : 1;
                timeSinceLastDirectionChange = 0f;
            }

            position.X += direction * enemySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (position.X < 0) position.X = 0;
            if (position.X > Game.GraphicsDevice.Viewport.Width - enemyTexture.Width)
                position.X = Game.GraphicsDevice.Viewport.Width - enemyTexture.Width;

            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastShot >= randomDelay)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }

        private void Shoot()
        {
            bulletSoundInstance.Volume = 0.25f;
            if (bulletSoundInstance.State != SoundState.Playing)
            {
                bulletSoundInstance.Play();
            }

            Vector2 bulletPosition1 = new Vector2(position.X + enemyTexture.Width / 2, position.Y);
            Vector2 bulletPosition2 = new Vector2(position.X + enemyTexture.Width / 2 - 50, position.Y);
            bullets.Add(new EnemyBullet(bulletTexture, bulletPosition1, this, 10));
            bullets.Add(new EnemyBullet(bulletTexture, bulletPosition2, this, 10));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, position, Color.White);
            DrawHealthBar(spriteBatch);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
            int barWidth = 50;
            int barHeight = 5;
            int healthBarOffset = 10;
            float healthPercentage = (float)health / maxHealth;
            int healthBarCurrentWidth = Math.Max(1, (int)(barWidth * healthPercentage)); // Avoid zero width
            Vector2 barPosition = new Vector2(position.X + (enemyTexture.Width - barWidth) / 2, position.Y + enemyTexture.Height + healthBarOffset);
            spriteBatch.Draw(Game1.CreateRectangleTexture(Game.GraphicsDevice, barWidth, barHeight, healthBarBackgroundColor), barPosition, Color.White);
            spriteBatch.Draw(Game1.CreateRectangleTexture(Game.GraphicsDevice, healthBarCurrentWidth, barHeight, healthBarColor), barPosition, Color.White);
        }
    }
}
