using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class MenuState : GameState
    {
        private SpriteFont font;

        public MenuState(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/PixelFont");
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game.ChangeState(new GameplayState(Game));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Press Enter to Start", new Vector2(500, 340), Color.White);
            spriteBatch.End();
        }
    }
}