using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using SpaceInvaders.Components;

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
        private List<Boss> bossList;
        private Texture2D enemyTexture;
        private Texture2D enemyTexture2;
        private Texture2D enemyBullet;
        private Texture2D enemyBullet2;
		private Texture2D bossTexture;
		private SpriteFont FPSfont;
        private int frameCount = 0;
        private float elapsedTime = 0f;
        private int fps = 0;

		private SpriteFont pixelfont;

        public int player_score=0;

		//dodalem pauzowanie gry
		private bool isPaused = false;
		private bool wasEscPressed = false;


        //powerup
        private List<PowerUp> powerUps; // Lista aktywnych power-upów
        private Texture2D healthPowerupTexture; // Tekstura power-upa
		private Texture2D fireRatePowerupTexture; // Tekstura power-upa szybkości strzelania
		private Texture2D shieldPowerupTexture; // Tekstura tarczy
		private Texture2D circleTexture;
		private Random random; // Generator losowy
        
        
        //sound
        private SoundEffect explosion = SoundEffect.FromFile("../../../Content/Sounds/enemy_boom.wav");
        private SoundEffectInstance explodeSound;

        private SoundEffect music = SoundEffect.FromFile("../../../Content/Music/CosmicConquest.wav");
        private SoundEffectInstance musicInstance;
        
		//LEVEL GENERATION
		private long randomPosX;
		private long randomPosY;

		private AnimatedExplosion animatedExplosion;
		private const float rotation = 0;
		private const float scale = 1;
		private const float depth = 0.5f;

		private Texture2D laserTexture;
		private List<Laser> lasers;

		private Button menuButton;
		private Texture2D buttonMenuTexture;

		private static int LEVEL { get; set; } = 3;
        
        public GameplayState(Game1 game) : base(game)
        {
	        bossList = new List<Boss>();
            bullets = new List<SingleBullet>();
            enemybullets = new List<EnemyBullet>();
			lasers = new List<Laser>();
			enemies = new List<Enemy1>();
            powerUps = new List<PowerUp>();
            random = new Random();
            musicInstance = music.CreateInstance();
            explodeSound = explosion.CreateInstance();
            explodeSound.Volume = 0.5f;
            animatedExplosion = new AnimatedExplosion(Vector2.Zero, rotation, scale, depth);
            
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
            
            enemyTexture2 = Game.Content.Load<Texture2D>("enemy2");
            enemyBullet2 = Game.Content.Load<Texture2D>("bullet2");

			buttonMenuTexture = Game.Content.Load<Texture2D>("Controls/buttonMenu");

			bossTexture = Game.Content.Load<Texture2D>("bossTexture");
			laserTexture = Game.Content.Load<Texture2D>("laserTexture");


			healthPowerupTexture = Game.Content.Load<Texture2D>("PowerUps/healthPowerup");
			fireRatePowerupTexture = Game.Content.Load<Texture2D>("PowerUps/fireRatePowerUp");
			shieldPowerupTexture = Game.Content.Load<Texture2D>("PowerUps/shieldPowerUp"); // Ścieżka do tekstury
			circleTexture = Game.Content.Load<Texture2D>("circleTexture");

			musicInstance.Volume = 0.1f;
			musicInstance.Play();
            FPSfont = Game.Content.Load<SpriteFont>("arial");
			pixelfont = Game.Content.Load<SpriteFont>("Fonts/PixelFont");

			// Inicjalizacja gracza
			player = new Player(playerTexture, new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 50, 620), bulletTexture, bullets, 100);


			menuButton = new Button(buttonMenuTexture)
				{
					Position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 160, Game.GraphicsDevice.Viewport.Height / 2 -120)

				};
				menuButton.Click += MenuButton_Click;

			


			addEnemies();
        }
		private void MenuButton_Click(object sender, System.EventArgs e)
		{
			musicInstance.Stop();
			// Przejście do następnego stanu gry
			Game.ChangeState(new MenuState(Game));
		}
		public override void Update(GameTime gameTime)
		{
			randomPosGen();
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
				menuButton.Update(gameTime); // Update the button when the game is paused
				return;
			}


			// Normalna aktualizacja gry
			if (player.health <= 0)
			{
				Game.ChangeState(new GameOverState(Game, player_score));

			}

			healthRectangle = new Rectangle(50, 20, player.health * 2, 40);
			healthBarRectangle = new Rectangle(50, 20, 200, 40);

			player.Update(gameTime);

			for (int i = enemies.Count - 1; i >= 0; i--)
			{
				if (enemies[i].health <= 0)
				{
					player_score += enemies[i].score;//dodanie punktów
													 // Szansa 15% na wygenerowanie power-upa
					if (random.Next(0, 100) < 99) // 15% szansy
					{
						PowerUpType type;
						int randomType = random.Next(0, 3); // Losowanie typu power-upa (0 = Health, 1 = FireRate, 2 = Shield)
						if (randomType == 0)
							type = PowerUpType.Health;
						else if (randomType == 1)
							type = PowerUpType.FireRate;
						else
							type = PowerUpType.Shield;

						Texture2D powerupTexture = type == PowerUpType.Health ? healthPowerupTexture :
												   type == PowerUpType.FireRate ? fireRatePowerupTexture :
												   shieldPowerupTexture;

						powerUps.Add(new PowerUp(powerupTexture, enemies[i].rectangle.Location.ToVector2(), type));
					}


					//odtwórz dźwięk wybuchu gdy znika przeciwnik
					explodeSound.Play();
					// Usuń przeciwnika po wykonaniu logiki
					enemies.RemoveAt(i);
				}
			}

			for (int i = powerUps.Count - 1; i >= 0; i--)
			{
				powerUps[i].Update(gameTime);

				if (powerUps[i].Rectangle.Intersects(player.rectangle))
				{
					if (powerUps[i].Type == PowerUpType.Health)
					{
						player.health += 20; // Leczenie
						if (player.health > 100) player.health = 100; // Maksymalny poziom zdrowia
					}
					else if (powerUps[i].Type == PowerUpType.FireRate)
					{
						player.ActivateFireRateBoost(15f); // Zwiększenie szybkości strzału
					}
					else if (powerUps[i].Type == PowerUpType.Shield)
					{
						player.ActivateShield(5f); // Aktywacja tarczy na 5 sekund
					}

					powerUps[i].Collect();
				}

				else if (powerUps[i].Rectangle.Y > Game.GraphicsDevice.Viewport.Height)
				{
					powerUps.RemoveAt(i); // Remove the Power-Up if it falls off-screen
				}
			}



			for (int i = bullets.Count - 1; i >= 0; i--)
			{
				bullets[i].Update(gameTime);

				for (int j = enemies.Count - 1; j >= 0; j--)
				{
					if (bullets[i].rectangle.Intersects(enemies[j].rectangle))
					{
						bossList[j].health -= 10;
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
			foreach (var boss in bossList)
			{
				boss.Update(gameTime);
			}


			for (int i = enemybullets.Count - 1; i >= 0; i--)
			{
				enemybullets[i].Update(gameTime);

					if (enemybullets[i].rectangle.Intersects(player.rectangle))
					{
						if (player.IsShieldActive) // Jeśli tarcza jest aktywna
						{
							// Tarcza pochłania pocisk, brak obrażeń
							enemybullets.RemoveAt(i); // Usuń pocisk po trafieniu tarczy
						}
						else
						{
							// Gracz otrzymuje obrażenia
							player.health -= enemybullets[i].damage;
							enemybullets.RemoveAt(i); // Usuń pocisk po trafieniu gracza
						}
					}
			}


			for (int i = lasers.Count - 1; i >= 0; i--)
			{
				lasers[i].Update(gameTime);
				if (lasers[i].IsExpired())
				{
					lasers.RemoveAt(i); // Usunięcie lasera po 3 sekundach
				}
				if (lasers[i].rectangle.Intersects(player.rectangle))
				{
					if (player.IsShieldActive)
					{
						lasers.RemoveAt(i);
					}
					else
					{
						player.health -= lasers[i].damage;
						lasers.RemoveAt(i);
					}
					
				}
			}


			for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (bullets[i].IsOffScreen())
                {
                    bullets.RemoveAt(i); 
                }
            }

			//SPRAWDZAM CZY PRZEZSZŁO SIĘ POZIOM
            if (enemies.Count == 0 || bossList.Count == 0)
            {
	            LEVEL++;
	            addEnemies();
            }

            if (player.health <= 0)
            {
	            musicInstance.Stop();
	            LEVEL = 1;
            }
            CalculateFPS(gameTime);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            spriteBatch.Draw(healthBarTexture, healthBarRectangle, Color.White);
			spriteBatch.DrawString(pixelfont, $"SCORE: {player_score}", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 75, 10), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f); //mozna skalowac napisy wow

			switch (getLEVEL())
            {
	            case 1:
		            spriteBatch.DrawString(pixelfont, "LEVEL1", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 700, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
		            break;
	            case 2:
		            spriteBatch.DrawString(pixelfont, "LEVEL2", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 700, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
		            break;
                case 3:
					spriteBatch.DrawString(pixelfont, "LEVEL3", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 700, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;

			}
			if (player.IsShieldActive)
			{
				// Rysuj okrągłą teksturę tarczy wokół gracza
				Vector2 shieldPosition = player.Position - new Vector2(circleTexture.Width / 2 -20, circleTexture.Height / 2 -25);
				spriteBatch.Draw(circleTexture, shieldPosition, Color.White);
			}
			player.Draw(spriteBatch);

            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
            foreach (var boss in bossList)
            {
	            boss.Draw(spriteBatch);
            }
            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (var enemyBullet in enemybullets)
            {
                enemyBullet.Draw(spriteBatch);
            }
            foreach (var laser in lasers)
			{
				laser.Draw(spriteBatch);
			}
			foreach (var powerUp in powerUps)
            {
                powerUp.Draw(spriteBatch);
            }

            spriteBatch.DrawString(FPSfont, $"FPS: {fps}", new Vector2(1815, 10), Color.Yellow);

			// Wyświetlanie komunikatu o pauzie
			if (isPaused)
			{
				spriteBatch.DrawString(pixelfont, "PAUSED", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 100, Game.GraphicsDevice.Viewport.Height / 2 -200), Color.Red);
				menuButton.Draw(spriteBatch);
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

        public void addEnemies()
        {
	        //SWITCH KTÓRY BĘDZIE DO PRZEŁĄCZANIA SCENARIUSZY DROPU PRZECIWNIKÓW!!!!!
	        switch (LEVEL)
	        {
		        case 1:
		        {
			        // Inicjalizacja przeciwników randomPosX = new Random().NextInt64(25, 500);
			        randomPosGen();
			        enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX, randomPosY), enemyBullet, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy1(enemyTexture,new Vector2(randomPosX,randomPosY), enemyBullet, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX,randomPosY), enemyBullet, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy2(enemyTexture2, new Vector2(randomPosX,randomPosY), enemyBullet2, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy2(enemyTexture2, new Vector2(randomPosX,randomPosY), enemyBullet2, enemybullets, Game));
			        break;
		        }
		        case 2:
		        {
			        // Inicjalizacja przeciwników
			        enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX, randomPosY), enemyBullet, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX,randomPosY), enemyBullet, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX, randomPosY), enemyBullet, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX, randomPosY), enemyBullet, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX, randomPosY), enemyBullet, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy2(enemyTexture2, new Vector2(randomPosX,randomPosY), enemyBullet2, enemybullets, Game));
			        randomPosGen();
			        enemies.Add(new Enemy2(enemyTexture2, new Vector2(randomPosX,randomPosY), enemyBullet2, enemybullets, Game));
			        break;
		        }
				case 3:
					{
						// Inicjalizacja przeciwników randomPosX = new Random().NextInt64(25, 500);
						randomPosGen();
						bossList.Add(new Boss(bossTexture, new Vector2(randomPosX, randomPosY),1000,  Game));
						
						break;
					}
				
			}
	        
        }

        public void randomPosGen()
        {
	        randomPosX = new Random().NextInt64(25, 500);
	        randomPosY = new Random().NextInt64(25, 250);
        }

        public int getLEVEL()
        {
	        return LEVEL;
        }
        
        
    }
}