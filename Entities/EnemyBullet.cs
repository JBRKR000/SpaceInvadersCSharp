using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class EnemyBullet
    {
        protected Vector2 position;
        protected Texture2D bulletTexture;
        protected float bulletSpeed = 600f;
		internal Rectangle rectangle;
        public int damage;


		public EnemyBullet(Texture2D bulletTexture, Vector2 startPosition,int damage)
        {
            this.position = startPosition;
            this.bulletTexture = bulletTexture;   
			this.damage = damage;
			this.rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height);
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, position, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
			

			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += bulletSpeed * deltaTime;
			rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height);
		}

        public bool IsOffScreen()
        {
            return position.Y < bulletTexture.Height;
        }
    }
}