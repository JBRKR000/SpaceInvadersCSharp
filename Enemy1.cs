
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
        private float timeSinceLastShot = 0f;
        private List<EnemyBullet> bullets;

        private Random random;
        private int direction = 1;
        private float directionChangeInterval = 3f;
        private float timeSinceLastDirectionChange = 0f;

        public int health = 50;


        public Enemy1(Texture2D enemyTexture, Vector2 position, Texture2D bulletTexture, List<EnemyBullet> bullets)
        {
            this.enemyTexture = enemyTexture;
            this.position = position;
            this.bulletTexture = bulletTexture;
            this.bullets = bullets;
            random = new Random();

			// Ustaw losowy czas między strzałami od 1 do 3 sekund
			shootInterval = (float)random.NextDouble() * 2f + 1f;

			// Ustaw losowe początkowe opóźnienie przed pierwszym strzałem
			timeSinceLastShot = (float)random.NextDouble() * shootInterval;

		}

        public void Draw(SpriteBatch spriteBatch)
        {
            if (health > 0)
			{
				spriteBatch.Draw(enemyTexture, position, Color.White);
			}
        }

        public void Update(GameTime gameTime)
        {
			rectangle = new Rectangle((int)position.X, (int)position.Y, enemyTexture.Width, enemyTexture.Height);


            //poruszanie sie
			timeSinceLastDirectionChange += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastDirectionChange >= directionChangeInterval)
            {
                direction = random.Next(0, 2) == 0 ? -1 : 1;
                timeSinceLastDirectionChange = 0f;
            }
            position.X += direction * enemySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (position.X < 0) position.X = 0;
            if (position.X > Game1.getScreenWidth() - enemyTexture.Width)
                position.X = Game1.getScreenWidth() - enemyTexture.Width;

            //strzelanie
            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastShot >= shootInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
				shootInterval = (float)random.NextDouble() * 2f + 1f; // Losowy czas od 1 do 3 sekund
			}
        }

        private void Shoot()
        {
            Vector2 bulletPosition1 = new Vector2(position.X + enemyTexture.Width / 2, position.Y);
            Vector2 bulletPosition2 = new Vector2(position.X + enemyTexture.Width / 2 - 60, position.Y);
            bullets.Add(new EnemyBullet(bulletTexture,bulletPosition1,this,10)); //przypisanie pociskow do przeciwnika
            bullets.Add(new EnemyBullet(bulletTexture,bulletPosition2,this,10)); 
        }
    }
}
