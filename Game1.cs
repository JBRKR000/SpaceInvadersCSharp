using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class Game1 : Game
    {
        private static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbs;
        private GameState _currentState;
        
        

        public Game1()
        {
            
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

        protected override void LoadContent()
        {
           
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ChangeState(new MenuState(this));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKbs = Keyboard.GetState();

            // Sprawdzanie, czy klawisz F2 został wciśnięty
            if (currentKbs.IsKeyDown(Keys.F2))
            {
               
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges(); // Zastosowanie zmiany
            }

            

            _currentState.Update(gameTime);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _currentState.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        public void ChangeState(GameState newState)
        {
            _currentState = newState;
            _currentState.LoadContent();
        }
        public static int getScreenWidth()
        {
            return _graphics.GraphicsDevice.Viewport.Width;
        }

        public static int getScreenHeight()
        {
            return _graphics.GraphicsDevice.Viewport.Height;
        }
        public static Texture2D CreateRectangleTexture(GraphicsDevice graphicsDevice, int width, int height, Color color)
        {
            if (width <= 0 || height <= 0)
            {
                
            }

            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i)
                data[i] = color;
            texture.SetData(data);
            return texture;
        }

    }
   
}
