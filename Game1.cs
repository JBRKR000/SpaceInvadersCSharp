
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D background;
        private Texture2D enemyTexture;
        private Texture2D enemyBullet;
        private Texture2D playerTexture;
        private Texture2D bulletTexture;
        private Player player;
        private List<SingleBullet> bullets;
        private List<EnemyBullet> enemybullets;
        private List<Enemy1> enemies;

        private SpriteFont FPSfont;
        private int frameCount = 0;
        private float elapsedTime = 0f;
        private int fps = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            bullets = new List<SingleBullet>();
            enemybullets = new List<EnemyBullet>();
            enemies = new List<Enemy1>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("wp");
            playerTexture = Content.Load<Texture2D>("player");
            bulletTexture = Content.Load<Texture2D>("bullet");
            enemyBullet = Content.Load<Texture2D>("bullet_enemy");
            enemyTexture = Content.Load<Texture2D>("enemy");

            FPSfont = Content.Load<SpriteFont>("arial");

            player = new Player(playerTexture,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height - 100), bulletTexture, bullets);

            enemies.Add(new Enemy1(enemyTexture, new Vector2(100, 100), enemyBullet, enemybullets));
            enemies.Add(new Enemy1(enemyTexture, new Vector2(400, 150), enemyBullet, enemybullets));
            enemies.Add(new Enemy1(enemyTexture, new Vector2(700, 200), enemyBullet, enemybullets));
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime);
                if (bullets[i].IsOffScreen())
                {
                    bullets.RemoveAt(i);
                }
            }
            for (int i = enemybullets.Count - 1; i >= 0; i--)
            {
                enemybullets[i].Update(gameTime);
                if (enemybullets[i].IsOffScreen())
                {
                    enemybullets.RemoveAt(i);
                }
            }
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }
            CalculateFPS(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);

            player.Draw(_spriteBatch);
            foreach (var bullet in bullets)
            {
                bullet.Draw(_spriteBatch);
            }
            foreach (var enemy in enemies)
            {
                enemy.Draw(_spriteBatch);
            }
            foreach (var bullet in enemybullets)
            {
                bullet.Draw(_spriteBatch);
            }

            _spriteBatch.DrawString(FPSfont, $"FPS: {fps}", new Vector2(10, 10), Color.Yellow);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CalculateFPS(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;

            if (elapsedTime >= 1f)
            {
                fps = frameCount;
                frameCount = 0;
                elapsedTime = 0f;
            }
        }

        public static int getScreenWidth()
        {
            return _graphics.GraphicsDevice.Viewport.Width;
        }
    }
}
