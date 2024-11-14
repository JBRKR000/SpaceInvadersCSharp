using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Player //41x51 takie są jego rozmiary (po teksturze)
    {
        private Texture2D playerTexture;
        private Vector2 startPosition;
        private float playerSpeed = 600f;

        private Texture2D bulletTexture;
        private List<SingleBullet> bullets;
        private float shootInterval = 0.25f;
        private float timeSinceLastShot = 0f;

        public int health=1000;

        public Rectangle rectangle; //cialo 


        public Player(Texture2D playerTexture, Vector2 startPosition, Texture2D bulletTexture, List<SingleBullet> bullets, int health)
        {
            this.playerTexture = playerTexture;
            this.startPosition = startPosition;
            this.bulletTexture = bulletTexture;
            this.bullets = bullets;
            this.health = health;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            

            if(health > 0){
				spriteBatch.Draw(playerTexture, startPosition, Color.White);
			}

            
        }

        public void Update(GameTime gameTime)
        {

			rectangle = new Rectangle((int)startPosition.X, (int)startPosition.Y, playerTexture.Width, playerTexture.Height);

			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			var keyboardState = Keyboard.GetState();

			timeSinceLastShot += deltaTime;
			if (health > 0)
			{
				// Ruch gracza
				if (keyboardState.IsKeyDown(Keys.A))
				{
					if (!(startPosition.X < 0))
					{
						startPosition.X -= playerSpeed * deltaTime;
					}
				}

				if (keyboardState.IsKeyDown(Keys.D))
				{
					if (!(startPosition.X >= Game1.getScreenWidth() - 50))
					{
						startPosition.X += playerSpeed * deltaTime;
					}
				}

				// Strzelanie, gdy gracz żyje
				if (keyboardState.IsKeyDown(Keys.Space) && timeSinceLastShot >= shootInterval)
				{
					Shoot();
					timeSinceLastShot = 0f;
				}
			}
		}

        private void Shoot()
        {
            Vector2 bulletPosition1 = new Vector2(startPosition.X + playerTexture.Width / 2, startPosition.Y);
            Vector2 bulletPosition2 = new Vector2(startPosition.X + playerTexture.Width / 2 - 40, startPosition.Y);
            bullets.Add(new SingleBullet(bulletTexture, bulletPosition1,10)); //kazdy bullet ma swoj dmg
            bullets.Add(new SingleBullet(bulletTexture, bulletPosition2,10));
        }
    }
}
