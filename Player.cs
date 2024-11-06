using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Player
    {
        private Texture2D playerTexture;
        private Vector2 startPosition;
        private float playerSpeed = 600f;
        private Texture2D bulletTexture;
        private List<SingleBullet> bullets;
        private bool isSpacePressed;
        private float shootInterval = 0.25f;
        private float timeSinceLastShot = 0f;
        private int health=100;

        public Player(Texture2D playerTexture, Vector2 startPosition, Texture2D bulletTexture, List<SingleBullet> bullets)
        {
            this.playerTexture = playerTexture;
            this.startPosition = startPosition;
            this.bulletTexture = bulletTexture;
            this.bullets = bullets;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, startPosition, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();

            timeSinceLastShot += deltaTime;
            
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (!(startPosition.X < 0))
                {
                    startPosition.X -= playerSpeed * deltaTime;    
                }
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (!(startPosition.X >= Game1.getScreenWidth()-50))
                {
                    startPosition.X += playerSpeed * deltaTime;   
                }
            }
            
            if (keyboardState.IsKeyDown(Keys.Space) && timeSinceLastShot >= shootInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }

        private void Shoot()
        {
            Vector2 bulletPosition1 = new Vector2(startPosition.X + playerTexture.Width / 2, startPosition.Y);
            Vector2 bulletPosition2 = new Vector2(startPosition.X + playerTexture.Width / 2 - 40, startPosition.Y);
            bullets.Add(new SingleBullet(bulletTexture, bulletPosition1));
            bullets.Add(new SingleBullet(bulletTexture, bulletPosition2));
        }
    }
}
