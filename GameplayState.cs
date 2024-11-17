using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class GameplayState : GameState
    {
        private SpriteBatch _spriteBatch;
        private Texture2D background;

        private Texture2D playerTexture;
        private Texture2D bulletTexture;
        private Player player;
        private List<SingleBullet> bullets;
        private Texture2D healthTexture;
        private Texture2D healthBarTexture;
        private Rectangle healthRectangle;
        private Rectangle healthBarRectangle;

        private List<EnemyBullet> enemybullets;
        private List<Enemy1> enemies;
        private Texture2D enemyTexture;
        private Texture2D enemyBullet;

        private SpriteFont FPSfont;
        private int frameCount = 0;
        private float elapsedTime = 0f;
        private int fps = 0;

		private SpriteFont pixelfont;


		//dodalem pauzowanie gry
		private bool isPaused = false;
		private bool wasEscPressed = false;


        //powerup
        private List<PowerUp> powerUps; // Lista aktywnych power-upów
        private Texture2D healthPowerupTexture; // Tekstura power-upa
        private Random random; // Generator losowy

        
        

        public GameplayState(Game1 game) : base(game)
        {
            bullets = new List<SingleBullet>();
            enemybullets = new List<EnemyBullet>();
            enemies = new List<Enemy1>();
            powerUps = new List<PowerUp>();
            random = new Random();

        }

        public override void LoadContent()
        {
           
			
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            background = Game.Content.Load<Texture2D>("wp");

            playerTexture = Game.Content.Load<Texture2D>("player");
            bulletTexture = Game.Content.Load<Texture2D>("bullet");
            healthTexture = Game.Content.Load<Texture2D>("healthTexture");
            healthBarTexture = Game.Content.Load<Texture2D>("healthBarTexture");

            enemyBullet = Game.Content.Load<Texture2D>("bullet_enemy");
            enemyTexture = Game.Content.Load<Texture2D>("enemy");

            healthPowerupTexture = Game.Content.Load<Texture2D>("PowerUps/healthPowerup");


            FPSfont = Game.Content.Load<SpriteFont>("arial");
			pixelfont = Game.Content.Load<SpriteFont>("Fonts/PixelFont");

			// Inicjalizacja gracza
			player = new Player(playerTexture, new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 50, 620), bulletTexture, bullets, 100);

            // Inicjalizacja przeciwników
            enemies.Add(new Enemy1(enemyTexture, new Vector2(100, 100), enemyBullet, enemybullets, Game));
            enemies.Add(new Enemy1(enemyTexture, new Vector2(400, 150), enemyBullet, enemybullets, Game));
            enemies.Add(new Enemy1(enemyTexture, new Vector2(700, 200), enemyBullet, enemybullets, Game));
        }

        public override void Update(GameTime gameTime)
        {
			KeyboardState keyboardState = Keyboard.GetState();

			// Sprawdzenie, czy gracz nacisnął ESC
			if (keyboardState.IsKeyDown(Keys.Escape) && !wasEscPressed)
			{
				isPaused = !isPaused; // Zmień stan pauzy
				wasEscPressed = true; // Oznacz, że ESC został wciśnięty
			}
			else if (keyboardState.IsKeyUp(Keys.Escape))
			{
				wasEscPressed = false; // Resetuj stan klawisza
			}

			if (isPaused)
			{
				return; // Jeśli gra jest wstrzymana, nie aktualizuj dalszych elementów
			}

			// Normalna aktualizacja gry
			if (player.health <= 0)
			{
				Game.ChangeState(new GameOverState(Game));
			}

			healthRectangle = new Rectangle(50, 20, player.health * 2, 40);
			healthBarRectangle = new Rectangle(50, 20, 200, 40);

			player.Update(gameTime);

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].health <= 0)
                {
                    // Szansa 15% na wygenerowanie power-upa
                    if (random.Next(0, 100) < 90)
                    {
                        powerUps.Add(new PowerUp(healthPowerupTexture, enemies[i].rectangle.Location.ToVector2()));
                    }
                    // Usuń przeciwnika po wykonaniu logiki
                    enemies.RemoveAt(i);
                }
            }

            for (int i = powerUps.Count - 1; i >= 0; i--)
            {
                powerUps[i].Update(gameTime);

                // Sprawdź kolizję z graczem
                if (powerUps[i].Rectangle.Intersects(player.rectangle))
                {
                    player.health += 20; // Leczenie gracza
                    if (player.health > 100) player.health = 100; // Maksymalne zdrowie 100
                    powerUps[i].Collect();
                }

                // Usuwanie zebranych lub poza ekranem
                if (powerUps[i].IsCollected)
                {
                    powerUps.RemoveAt(i);
                }
            }


            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime);

                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    if (bullets[i].rectangle.Intersects(enemies[j].rectangle))
                    {
                        enemies[j].health -= 10; 
                        bullets.RemoveAt(i); 
                        break; 
                    }
                }
            }

           

            
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }
            

            for (int i = enemybullets.Count - 1; i >= 0; i--)
			{
				enemybullets[i].Update(gameTime);
				if (enemybullets[i].rectangle.Intersects(player.rectangle))
				{
					player.health -= enemybullets[i].damage;
					enemybullets.RemoveAt(i); // Usunięcie pocisku po trafieniu gracza
				}
			}


			for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (bullets[i].IsOffScreen())
                {
                    bullets.RemoveAt(i); 
                }
            }

            CalculateFPS(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            spriteBatch.Draw(healthBarTexture, healthBarRectangle, Color.White);

            player.Draw(spriteBatch);

            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (var enemyBullet in enemybullets)
            {
                enemyBullet.Draw(spriteBatch);
            }
            foreach (var powerUp in powerUps)
            {
                powerUp.Draw(spriteBatch);
            }

            spriteBatch.DrawString(FPSfont, $"FPS: {fps}", new Vector2(1215, 10), Color.Yellow);

			// Wyświetlanie komunikatu o pauzie
			if (isPaused)
			{
				spriteBatch.DrawString(pixelfont, "PAUSED", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 100, Game.GraphicsDevice.Viewport.Height / 2), Color.Red);
			}


			spriteBatch.End();
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
    }
}
