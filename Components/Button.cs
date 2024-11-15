using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceInvaders.Components
{
	internal class Button : Component
	{
		private MouseState _currentMouse; // Aktualny stan myszy (lewy przycisk)
		private MouseState _previousMouse; // Poprzedni stan myszy (prawy przycisk)
		private Texture2D _texture; // Tekstura przycisku

		public event EventHandler Click; // Event kliknięcia

		public bool Clicked { get; private set; }

		public Vector2 Position { get; set; }

		// Prostokąt, który reprezentuje położenie i rozmiar przycisku
		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
			}
		}

		// Konstruktor, który przyjmuje teksturę przycisku
		public Button(Texture2D texture)
		{
			_texture = texture; // Tekstura przycisku
		}

		// Rysowanie przycisku
		public override void Draw(SpriteBatch spriteBatch)
		{
			var color = Color.White;
			spriteBatch.Draw(_texture, Rectangle, color); // Rysowanie tekstury przycisku
		}

		// Aktualizacja przycisku (sprawdzenie kliknięcia)
		public override void Update(GameTime gameTime)
		{
			_previousMouse = _currentMouse;
			_currentMouse = Mouse.GetState();

			// Sprawdzenie, czy kursor myszy znajduje się w obrębie przycisku
			var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

			if (mouseRectangle.Intersects(Rectangle))
			{
				if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
				{
					// Wywołanie eventu kliknięcia
					Click?.Invoke(this, new EventArgs());
				}
			}
		}
	}
}
