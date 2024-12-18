﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using SpaceInvaders.Components;
using SpaceInvaders.Entities;
using System.Reflection.Emit;

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
        private Boss boss;
		private bool isBossAlive;
        private Texture2D enemyTexture;
        private Texture2D enemyTexture2;
        private Texture2D enemyTexture3;
        private Texture2D enemyTexture4;
        private Texture2D enemyTexture5;
        private Texture2D enemyBullet;
        private Texture2D enemyBullet2;
        private Texture2D enemyBullet3;
        private Texture2D enemyBullet4;
		private Texture2D enemyBullet5;
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
        
        private SoundEffect enemy_hitsound = SoundEffect.FromFile("../../../Content/Sounds/enemy_hit.wav");
        private SoundEffectInstance enemy_hitsound_instance;

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


		public static Boolean godmode;
		private static int LEVEL { get; set; }

        private Vector2 previousBossPosition; // Przechowuje poprzednią pozycję bossa
        private bool laserActive; // Czy laser jest aktywny
        private Laser bossLaser;
        public GameplayState(Game1 game) : base(game)
        {
	        godmode = false;
            bullets = new List<SingleBullet>();
            enemybullets = new List<EnemyBullet>();
			lasers = new List<Laser>();
			enemies = new List<Enemy1>();
            powerUps = new List<PowerUp>();
            random = new Random();
            musicInstance = music.CreateInstance();
            explodeSound = explosion.CreateInstance();
            enemy_hitsound_instance = enemy_hitsound.CreateInstance();
            explodeSound.Volume = 0.5f;
            animatedExplosion = new AnimatedExplosion(Vector2.Zero, rotation, scale, depth);
            LEVEL = 1;
            player_score = 0;
            bullets.Clear();
            enemybullets.Clear();
            enemies.Clear();
            laserActive = false;
            bossLaser = null;
            previousBossPosition = Vector2.Zero;
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


			enemyTexture3 = Game.Content.Load<Texture2D>("enemy3");
			enemyBullet3 = Game.Content.Load<Texture2D>("bullet3");
			
			
			enemyTexture4 = Game.Content.Load<Texture2D>("enemy4");
			enemyBullet4 = Game.Content.Load<Texture2D>("bulletx");

			enemyBullet5 = Game.Content.Load<Texture2D>("bullet5");
			enemyTexture5 = Game.Content.Load<Texture2D>("enemy5");


			buttonMenuTexture = Game.Content.Load<Texture2D>("Controls/buttonMenu");

			bossTexture = Game.Content.Load<Texture2D>("bossTexture");
			laserTexture = Game.Content.Load<Texture2D>("laserTexture");


			healthPowerupTexture = Game.Content.Load<Texture2D>("PowerUps/healthPowerup");
			fireRatePowerupTexture = Game.Content.Load<Texture2D>("PowerUps/fireRatePowerUp");
			shieldPowerupTexture = Game.Content.Load<Texture2D>("PowerUps/shieldPowerUp"); // Ścieżka do tekstury
			circleTexture = Game.Content.Load<Texture2D>("circleTexture");

			musicInstance.Volume = 0.1f;
			musicInstance.Play();
			musicInstance.IsLooped = true;
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
			if (keyboardState.IsKeyDown(Keys.G))
			{
				godmode = !godmode;
			}
			
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
            if (isBossAlive && boss != null && boss.health <= 0)
            {
                isBossAlive = false;
            }


            if (isBossAlive && boss != null)
            {
                // Zaktualizowanie pozycji lasera na podstawie pozycji bossa
                if (laserActive)
                {
                    // Ustawienie nowej pozycji lasera na podstawie pozycji bossa
                    bossLaser.Position = new Vector2(boss.position.X + boss.width / 2 - laserTexture.Width / 2, boss.position.Y + boss.height / 2);
                }

                // Sprawdzamy, czy boss stoi, aby stworzyć laser
                if (boss.position.Y == previousBossPosition.Y && !laserActive) // Jeśli boss stoi, a laser jeszcze nie został stworzony
                {
                    laserActive = true;
                    Vector2 laserPosition = new Vector2(boss.position.X + boss.width / 2 - laserTexture.Width / 2, boss.position.Y + boss.height / 2 +100);
                    bossLaser = new Laser(laserTexture, laserPosition, new Vector2(0, 0)); // Prędkość (0, 0) – brak ruchu w dół
                }

                // Jeśli boss się porusza, dezaktywujemy laser
                if (boss.position.Y != previousBossPosition.Y)
                {
                    laserActive = false;
                    bossLaser = null; // Usunięcie lasera
                }

                previousBossPosition = boss.position; // Zapamiętanie poprzedniej pozycji bossa
            }



            // Aktualizacja lasera, jeśli aktywny
            if (laserActive && bossLaser != null)
            {
                bossLaser.Update(gameTime);
            }

            for (int i = lasers.Count - 1; i >= 0; i--)
            {
                lasers[i].Update(gameTime); // Aktualizujemy laser

                // Sprawdzamy, czy laser koliduje z graczem
                if (lasers[i].rectangle.Intersects(player.rectangle))
                {
                    player.health -= lasers[i].damage; // Zadajemy obrażenia graczowi
                    lasers.RemoveAt(i); // Usuwamy laser po trafieniu
                }
                else if (lasers[i].IsOffScreen()) // Usuwamy laser, jeśli opuścił ekran
                {
                    lasers.RemoveAt(i);
                }
            }



            healthRectangle = new Rectangle(50, 20, player.health * 2, 40);
			healthBarRectangle = new Rectangle(50, 20, 200, 40);

			player.Update(gameTime);

			for (int i = enemies.Count - 1; i >= 0; i-- )
			{
					if (enemies[i].health <= 0)
					{
						player_score += enemies[i].score;//dodanie punktów
                                                         // Szansa 15% na wygenerowanie power-upa
                        if (random.Next(0, 100) < 60) // 60% szansy
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
						enemy_hitsound.Play();
						enemies[j].health -= 10;
						bullets.RemoveAt(i);
						break;
					}
                }
                if (isBossAlive && bullets[i].rectangle.Intersects(boss.Rectangle))
                {
	                enemy_hitsound.Play();
                    boss.health -= bullets[i].damage;
                    bullets.RemoveAt(i);
                    if (boss.health <= 0)
                    {
                        isBossAlive = false;
                        player_score += boss.score;
                    }
                    break;
                }
            }

           




            foreach (var enemy in enemies)
			{
				enemy.Update(gameTime);
			}
            if (isBossAlive && boss != null)
            {
                boss.Update(gameTime);

                if (boss.health <= 0)
                {
                    isBossAlive = false;
                    player_score += boss.score;
                    boss = null; // Usunięcie bossa po pokonaniu
                    explodeSound.Play();
                }
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



			for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (bullets[i].IsOffScreen())
                {
                    bullets.RemoveAt(i); 
                }
            }

            //SPRAWDZAM CZY PRZEZSZŁO SIĘ POZIOM
            if (!isBossAlive && enemies.Count == 0)
            {
                LEVEL++;
                addEnemies();
            }

            if (player.health <= 0)
            {
	            musicInstance.Stop();
	            LEVEL = 1;
                Game.ChangeState(new GameOverState(Game, player_score));
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
		            spriteBatch.DrawString(pixelfont, "LEVEL1", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
		            break;
	            case 2:
		            background = Game.Content.Load<Texture2D>("Backgrounds/background2");
		            spriteBatch.DrawString(pixelfont, "LEVEL2", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
		            break;
                case 3:
		            background = Game.Content.Load<Texture2D>("Backgrounds/background3");
					spriteBatch.DrawString(pixelfont, "LEVEL3", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;
				case 4:
					spriteBatch.DrawString(pixelfont, "LEVEL4", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;
				case 5:
					background = Game.Content.Load<Texture2D>("Backgrounds/background2");
					spriteBatch.DrawString(pixelfont, "LEVEL5", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;
				case 6:
					background = Game.Content.Load<Texture2D>("Backgrounds/background3");
					spriteBatch.DrawString(pixelfont, "LEVEL6", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;
				case 7:
					spriteBatch.DrawString(pixelfont, "LEVEL7", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;
				case 8:
					background = Game.Content.Load<Texture2D>("Backgrounds/background2");
					spriteBatch.DrawString(pixelfont, "LEVEL8", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;
				case 9:
					background = Game.Content.Load<Texture2D>("Backgrounds/background3");
					spriteBatch.DrawString(pixelfont, "LEVEL9", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;
				case 10:
					background = Game.Content.Load<Texture2D>("Backgrounds/background3");
					spriteBatch.DrawString(pixelfont, "LEVEL10", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 + 600, Game.GraphicsDevice.Viewport.Height - 75), Color.Red);
					break;

			}
			if (player.IsShieldActive)
			{
				// Rysuj okrągłą teksturę tarczy wokół gracza
				Vector2 shieldPosition = player.Position - new Vector2(circleTexture.Width / 2 -20, circleTexture.Height / 2 -25);
				spriteBatch.Draw(circleTexture, shieldPosition, Color.White);
			}
			player.Draw(spriteBatch);
            if (isBossAlive && boss != null)
            {
                boss.Draw(spriteBatch);
            }

            if (laserActive && bossLaser != null)
            {
                bossLaser.Draw(spriteBatch);
            }

            if (laserActive && bossLaser != null)
            {
                if (bossLaser.rectangle.Intersects(player.rectangle))
                {
                    player.health -= bossLaser.damage; // Zadajemy obrażenia graczowi
                    laserActive = false; // Zatrzymujemy laser
                    bossLaser = null; // Usuwamy laser po trafieniu gracza
                }
            }


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
            isBossAlive = false; // Reset na początku każdego poziomu
            boss = null;         // Upewnienie się, że boss jest usunięty

			switch (LEVEL)
			{
				case 1:
					{
						// Poziom 1: tylko Enemy1
						for (int i = 0; i < 6; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX, randomPosY), enemyBullet, enemybullets, Game));
						}
						break;
					}
				case 2:
					{
						// Poziom 2: tylko Enemy2
						for (int i = 0; i < 5; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy2(enemyTexture2, new Vector2(randomPosX, randomPosY), enemyBullet2, enemybullets, Game));
						}
						break;
					}
				case 3:
					{
						// Poziom 3: mieszanka Enemy1 i Enemy2
						for (int i = 0; i < 4; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX, randomPosY), enemyBullet, enemybullets, Game));
						}
						for (int i = 0; i < 3; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy2(enemyTexture2, new Vector2(randomPosX, randomPosY), enemyBullet2, enemybullets, Game));
						}
						break;
					}
				case 4:
					{
						// Poziom 4: dużo Enemy3 (słabi, ale szybcy)
						for (int i = 0; i < 10; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy3(enemyTexture3, new Vector2(randomPosX, randomPosY), enemyBullet3, enemybullets, Game));
						}
						break;
					}
				case 5:
					{
						// Poziom 5: Enemy1, Enemy2 i Enemy3
						for (int i = 0; i < 3; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy1(enemyTexture, new Vector2(randomPosX, randomPosY), enemyBullet, enemybullets, Game));
						}
						for (int i = 0; i < 3; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy2(enemyTexture2, new Vector2(randomPosX, randomPosY), enemyBullet2, enemybullets, Game));
						}
						for (int i = 0; i < 5; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy3(enemyTexture3, new Vector2(randomPosX, randomPosY), enemyBullet3, enemybullets, Game));
						}
						break;
					}
				case 6:
					{
						// Poziom 6: wprowadzenie Enemy4
						for (int i = 0; i < 4; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy3(enemyTexture3, new Vector2(randomPosX, randomPosY), enemyBullet3, enemybullets, Game));
						}
						for (int i = 0; i < 2; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy4(enemyTexture4, new Vector2(randomPosX, randomPosY), enemyBullet4, enemybullets, Game));
						}
						break;
					}
				case 7:
					{
						// Poziom 7: mieszanka Enemy3 i Enemy4
						for (int i = 0; i < 6; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy3(enemyTexture3, new Vector2(randomPosX, randomPosY), enemyBullet3, enemybullets, Game));
						}
						for (int i = 0; i < 3; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy4(enemyTexture4, new Vector2(randomPosX, randomPosY), enemyBullet4, enemybullets, Game));
						}
						break;
					}
				case 8:
					{
						// Poziom 8: wprowadzenie Enemy5
						for (int i = 0; i < 3; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy4(enemyTexture4, new Vector2(randomPosX, randomPosY), enemyBullet4, enemybullets, Game));
						}
						for (int i = 0; i < 2; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy5(enemyTexture5, new Vector2(randomPosX, randomPosY), enemyBullet5, enemybullets, Game));
						}
						break;
					}
				case 9:
					{
						// Poziom 9: dużo Enemy5
						for (int i = 0; i < 5; i++)
						{
							randomPosGen();
							enemies.Add(new Enemy5(enemyTexture5, new Vector2(randomPosX, randomPosY), enemyBullet5, enemybullets, Game));
						}
						break;
					}
				case 10:
					{
						// Poziom 10: Boss
						isBossAlive = true;
						randomPosGen();
						boss = new Boss(bossTexture, new Vector2(randomPosX, randomPosY), 1000, Game, enemybullets, enemyBullet, 3000);
						break;
					}
			}

		}

		public void randomPosGen()
        {
	        randomPosX = new Random().NextInt64(25, 1500);
	        randomPosY = new Random().NextInt64(25, 350);
        }

        public int getLEVEL()
        {
	        return LEVEL;
        }
        
        
    }
}