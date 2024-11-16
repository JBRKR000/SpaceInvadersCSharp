using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Components;

namespace SpaceInvaders
{
    public class GameOverState : GameState
    {
        private SpriteFont font;
		private Texture2D buttonResetTexture;
		private Button resetButton;
		public GameOverState(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/PixelFont");
			buttonResetTexture = Game.Content.Load<Texture2D>("Controls/buttonReset");
			resetButton = new Button(buttonResetTexture)
			{
				Position = new Vector2(590, 540)

			};

			resetButton.Click += ResetButton_Click;
		}
		private void ResetButton_Click(object sender, System.EventArgs e)
		{
			// Przejście do następnego stanu gry
			Game.ChangeState(new GameplayState(Game));
		}

		public override void Update(GameTime gameTime)
        {
			resetButton.Update(gameTime);
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.DarkRed);
            spriteBatch.Begin();
			
			resetButton.Draw(spriteBatch);
			spriteBatch.End();
        }
    }
}