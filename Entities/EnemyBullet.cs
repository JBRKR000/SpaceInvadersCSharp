using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class EnemyBullet
    {
        private Vector2 position;
        private Texture2D bulletTexture;
        private float bulletSpeed = 600f;
		internal Rectangle rectangle;
        public int damage;
		public Enemy1 owner;


		public EnemyBullet(Texture2D bulletTexture, Vector2 startPosition, Enemy1 owner,int damage)
        {
            this.position = startPosition;
            this.bulletTexture = bulletTexture;
            this.owner = owner;
			this.damage = damage;
			this.rectangle = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height);
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, position, Color.White);
        }

        public void Update(GameTime gameTime)
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