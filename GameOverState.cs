using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Components;

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

        public override void LoadContent()
        {
			background = Game.Content.Load<Texture2D>("Backgrounds/deadBackground");
			font = Game.Content.Load<SpriteFont>("Fonts/PixelFont");
			buttonResetTexture = Game.Content.Load<Texture2D>("Controls/buttonReset");
			buttonMenuTexture = Game.Content.Load<Texture2D>("Controls/buttonMenu");
			resetButton = new Button(buttonResetTexture)
			{
				Position = new Vector2(690, 540)

			};

			resetButton.Click += ResetButton_Click;

			menuButton = new Button(buttonMenuTexture)
			{
				Position = new Vector2(290, 540)

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
			resetButton.Draw(spriteBatch);
			menuButton.Draw(spriteBatch);
			spriteBatch.End();
        }
    }
}