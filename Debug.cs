using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class DebugWindow : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private List<string> _variables;
        private List<float> _values;
        private int _selectedIndex = 0;
        private KeyboardState _previousKeyboardState;

        public DebugWindow(SpriteFont font)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _font = font;
            _variables = new List<string>();
            _values = new List<float>();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Arial"); // upewnij się, że masz odpowiednią czcionkę
        }

        public void AddVariable(string name, ref float value)
        {
            _variables.Add(name);
            _values.Add(value);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Przełączanie między zmiennymi
            if (keyboardState.IsKeyDown(Keys.Down) && _previousKeyboardState.IsKeyUp(Keys.Down))
            {
                _selectedIndex = (_selectedIndex + 1) % _variables.Count;
            }
            if (keyboardState.IsKeyDown(Keys.Up) && _previousKeyboardState.IsKeyUp(Keys.Up))
            {
                _selectedIndex = (_selectedIndex - 1 + _variables.Count) % _variables.Count;
            }

            // Zmiana wartości
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _values[_selectedIndex] -= 0.1f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _values[_selectedIndex] += 0.1f;
            }

            _previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            for (int i = 0; i < _variables.Count; i++)
            {
                Color color = i == _selectedIndex ? Color.Yellow : Color.White;
                _spriteBatch.DrawString(_font, $"{_variables[i]}: {_values[i]:0.00}", new Vector2(10, 20 + i * 20), color);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
