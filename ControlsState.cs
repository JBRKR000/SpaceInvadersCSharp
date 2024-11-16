using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
	internal class ControlsState : GameState
	{
		private Texture2D buttonBackTexture;
		private Button backButton;
		private Texture2D background;

		public ControlsState(Game1 game) : base(game){}
		public override void LoadContent()
		{
			background = Game.Content.Load<Texture2D>("Backgrounds/controls1player");
			buttonBackTexture = Game.Content.Load<Texture2D>("Controls/buttonBack");
			backButton = new Button(buttonBackTexture)
			{
				Position = new Vector2(510, 540)

			};

			backButton.Click += BackButton_Click;
		}

		private void BackButton_Click(object sender, System.EventArgs e)
		{
			// Przejście do następnego stanu gry
			Game.ChangeState(new MenuState(Game));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
			spriteBatch.Begin();
			spriteBatch.Draw(background, Game.GraphicsDevice.Viewport.Bounds, Color.White);
			backButton.Draw(spriteBatch);
			spriteBatch.End();
		}

	

		public override void Update(GameTime gameTime)
		{
			backButton.Update(gameTime);
		}
	}
}
