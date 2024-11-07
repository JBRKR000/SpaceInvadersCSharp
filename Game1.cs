
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class Game1 : Game
    {
        //world
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D background;
        
        //player
        private Texture2D playerTexture;
        private Texture2D bulletTexture;
        private Player player;
        private List<SingleBullet> bullets;
		private Texture2D healthTexture;
		private Rectangle healthRectangle;

		//enemy
		private List<EnemyBullet> enemybullets;
        private List<Enemy1> enemies;
		private Texture2D enemyTexture;
		private Texture2D enemyBullet;

		//licznik fps
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

            //world
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("wp");

            //player
            playerTexture = Content.Load<Texture2D>("player");
            bulletTexture = Content.Load<Texture2D>("bullet");
			healthTexture = Content.Load<Texture2D>("health");

			//enemy
			enemyBullet = Content.Load<Texture2D>("bullet_enemy");
            enemyTexture = Content.Load<Texture2D>("enemy");

            //fps
            FPSfont = Content.Load<SpriteFont>("arial");

            player = new Player(playerTexture,
                new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height - 100), bulletTexture, bullets,100);

            



            enemies.Add(new Enemy1(enemyTexture, new Vector2(100, 100), enemyBullet, enemybullets));
            enemies.Add(new Enemy1(enemyTexture, new Vector2(400, 150), enemyBullet, enemybullets));
            enemies.Add(new Enemy1(enemyTexture, new Vector2(700, 200), enemyBullet, enemybullets));
        }

        protected override void Update(GameTime gameTime)
        {
            if (player.health <= 0)
            {
                //mozemy tu dodac przelaczanie na jakas scene game over gdy gracz umrze
            }
           
            healthRectangle = new Rectangle(50,20, player.health, 20);
            player.Update(gameTime);

			var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();


			for (int i = bullets.Count - 1; i >= 0; i--)
			{
				bullets[i].Update(gameTime);
				if (bullets[i].IsOffScreen())
				{
					bullets.RemoveAt(i);
				}
				else
				{
					for (int j = enemies.Count - 1; j >= 0; j--)
					{
						if (bullets[i].rectangle.Intersects(enemies[j].rectangle))
						{
							enemies[j].health -= bullets[i].damage;
							bullets.RemoveAt(i);
							i--;
							if (enemies[j].health <= 0)
							{
								// Usuń przeciwnika i jego pociski
								enemybullets.RemoveAll(bullet => bullet.owner == enemies[j]);
								enemies.RemoveAt(j);
							}
							break;
						}
					}
				}
			}
			for (int i = enemybullets.Count - 1; i >= 0; i--)
            {
                enemybullets[i].Update(gameTime);
                if (enemybullets[i].IsOffScreen())
                {
                    enemybullets.RemoveAt(i);
                }
				else if (enemybullets[i].rectangle.Intersects(player.rectangle)) 
				{
					player.health -= enemybullets[i].damage; 
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

            _spriteBatch.Draw(healthTexture,healthRectangle, Color.White);
            player.Draw(_spriteBatch);


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

            _spriteBatch.DrawString(FPSfont, $"FPS: {fps}", new Vector2(1215, 10), Color.Yellow);
           
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
