using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class DebugWindow
    {
        private SpriteFont _font;
        private List<string> _variables;
        private List<float> _values;
        private int _selectedIndex = 0;
        private KeyboardState _previousKeyboardState;

        public DebugWindow(SpriteFont font)
        {
            _font = font;
            _variables = new List<string>();
            _values = new List<float>();
        }

        // Dodaj zmienną do debugowania
        public void AddVariable(string name, ref float value)
        {
            _variables.Add(name);
            _values.Add(value);
        }

        // Aktualizacja debug okna (np. obsługa klawiatury)
        public void Update()
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
        }

        // Rysowanie debug okna
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _variables.Count; i++)
            {
                Color color = i == _selectedIndex ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, $"{_variables[i]}: {_values[i]:0.00}", new Vector2(10, 20 + i * 20), color);
            }
        }

        // Pobierz wartość zmiennej
        public float GetValue(int index)
        {
            return _values[index];
        }
    }
}
