using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class SingleBullet
    {
        private Vector2 position;
        private Texture2D bulletTexture;
        private float bulletSpeed = 750f;
        public int damage;
        public Rectangle rectangle;

        public SingleBullet(Texture2D bulletTexture, Vector2 startPosition, int damage)
        {
            this.position = startPosition;
            this.bulletTexture = bulletTexture;
            this.damage = damage; //damage pocisku, bedzie mozna dodac jakies buffy do pociskow
			this.rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height); //hitbox pocisku
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y -= bulletSpeed * deltaTime;
			rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height); // aktualizacja hitboxa
		}

        public bool IsOffScreen()
        {
            return position.Y < -bulletTexture.Height;
        }
    }
}