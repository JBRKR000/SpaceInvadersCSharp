using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Components;

namespace SpaceInvaders
{
	public class MenuState : GameState
	{
		private SpriteFont font;
		private Texture2D buttonPlayTexture;
		private Texture2D background;
		private Button startButton;

		public MenuState(Game1 game) : base(game) { }

		public override void LoadContent()
		{
			font = Game.Content.Load<SpriteFont>("Fonts/PixelFont");
			buttonPlayTexture = Game.Content.Load<Texture2D>("Controls/buttonPlay");
			background = Game.Content.Load<Texture2D>("Backgrounds/menuBackground");
			// Inicjalizacja guzika
			startButton = new Button(buttonPlayTexture)
			{
				Position = new Vector2(590, 240),
				
			};

			startButton.Click += StartButton_Click;
		}

		private void StartButton_Click(object sender, System.EventArgs e)
		{
			// Przejście do następnego stanu gry
			Game.ChangeState(new GameplayState(Game));
		}

		public override void Update(GameTime gameTime)
		{
			startButton.Update(gameTime);

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

			spriteBatch.Begin();
			spriteBatch.Draw(background, Game.GraphicsDevice.Viewport.Bounds, Color.White);


			// Rysowanie guzika
			startButton.Draw(spriteBatch);

			spriteBatch.End();
		}
	}
}
