using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Components;
using System;

namespace SpaceInvaders
{
    public class GameOverState : GameState
    {
		private Texture2D background;
		private SpriteFont font;
		private Texture2D buttonResetTexture;
		private Texture2D buttonMenuTexture;
		private Button resetButton;
		private Button menuButton;
		public GameOverState(Game1 game) : base(game) { }
		private SpriteFont pixelfont;
		private int finalScore;
		private int highScore;

		private string highScoreFilePath = "../highscore.txt";

		private int LoadHighScore()
		{
			if (System.IO.File.Exists(highScoreFilePath))
			{
				string content = System.IO.File.ReadAllText(highScoreFilePath);
				if (int.TryParse(content, out int highScore))
				{
					return highScore;
				}
			}
			return 0; // Jeśli plik nie istnieje lub nie można go odczytać
		}

		private void SaveHighScore(int score)
		{
			System.IO.File.WriteAllText(highScoreFilePath, score.ToString());
		}

		

		public GameOverState(Game1 game, int score) : base(game)
		{
			finalScore = score; // Wynik końcowy
			highScore = LoadHighScore(); // Załaduj najlepszy wynik

			// Zaktualizuj najlepszy wynik, jeśli obecny jest większy
			if (finalScore > highScore)
			{
				highScore = finalScore;
				SaveHighScore(highScore); // Zapisz nowy najlepszy wynik
			}
            

        }

        public override void LoadContent()
        {
			background = Game.Content.Load<Texture2D>("Backgrounds/deadBackground");
			font = Game.Content.Load<SpriteFont>("Fonts/PixelFont");
			buttonResetTexture = Game.Content.Load<Texture2D>("Controls/buttonReset");
			buttonMenuTexture = Game.Content.Load<Texture2D>("Controls/buttonMenu");
			pixelfont = Game.Content.Load<SpriteFont>("Fonts/PixelFont");

            int screenWidth = Game.GraphicsDevice.Viewport.Width;
            int screenHeight = Game.GraphicsDevice.Viewport.Height;

            // Środek ekranu
            float centerX = screenWidth / 2f;

            resetButton = new Button(buttonResetTexture)
			{
                Position = new Vector2(centerX - buttonResetTexture.Width / 2f - 175, 640),

            };

			resetButton.Click += ResetButton_Click;

			menuButton = new Button(buttonMenuTexture)
			{
				Position = new Vector2(centerX - buttonMenuTexture.Width / 2f +175, 640),

            };

			menuButton.Click += MenuButton_Click;




		}
		private void ResetButton_Click(object sender, System.EventArgs e)
		{
			// Przejście do następnego stanu gry
			Game.ChangeState(new GameplayState(Game));
		}
		private void MenuButton_Click(object sender, System.EventArgs e)
		{
			// Przejście do następnego stanu gry
			Game.ChangeState(new MenuState(Game));
		}
		public override void Update(GameTime gameTime)
        {
			resetButton.Update(gameTime);
			menuButton.Update(gameTime);
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Begin();
			spriteBatch.Draw(background, Game.GraphicsDevice.Viewport.Bounds, Color.White);
			spriteBatch.DrawString(pixelfont, $"FINAL SCORE: {finalScore}", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 240, 470), Color.Yellow, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f); //mozna skalowac napisy wow
			spriteBatch.DrawString(
				pixelfont,
				$"HIGH SCORE: {highScore}",
				new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 240, 420),
				Color.Cyan,
				0f,
				Vector2.Zero,
				1.2f,
				SpriteEffects.None,
				0f
			);

			resetButton.Draw(spriteBatch);
			menuButton.Draw(spriteBatch);
			spriteBatch.End();
        }











		
	}
}