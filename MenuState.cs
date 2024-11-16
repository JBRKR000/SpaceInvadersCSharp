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
		private Texture2D buttonControlsTexture;
		private Texture2D background;
		private Button playButton;
		private Button controlsButton;

		public MenuState(Game1 game) : base(game) { }

		public override void LoadContent()
		{
			font = Game.Content.Load<SpriteFont>("Fonts/PixelFont");
			buttonPlayTexture = Game.Content.Load<Texture2D>("Controls/buttonPlay");
			buttonControlsTexture = Game.Content.Load<Texture2D>("Controls/buttonControls");
			background = Game.Content.Load<Texture2D>("Backgrounds/menuBackground");
			// Inicjalizacja guzika
			playButton = new Button(buttonPlayTexture)
			{
				Position = new Vector2(590, 240),
				
			};

			playButton.Click += StartButton_Click;


			controlsButton = new Button(buttonControlsTexture)
			{
				Position = new Vector2(510, 300),

			};

			controlsButton.Click += ControlsButton_Click;

		}

		private void StartButton_Click(object sender, System.EventArgs e)
		{
			// Przejście do następnego stanu gry
			Game.ChangeState(new GameplayState(Game));
		}

		private void ControlsButton_Click(object sender, System.EventArgs e)
		{
			// Przejście do następnego stanu gry
			Game.ChangeState(new ControlsState(Game));
		}

		public override void Update(GameTime gameTime)
		{
			playButton.Update(gameTime);
			controlsButton.Update(gameTime);

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

			spriteBatch.Begin();
			spriteBatch.Draw(background, Game.GraphicsDevice.Viewport.Bounds, Color.White);


			// Rysowanie guzika
			playButton.Draw(spriteBatch);
			controlsButton.Draw(spriteBatch);


			spriteBatch.End();
		}
	}
}
