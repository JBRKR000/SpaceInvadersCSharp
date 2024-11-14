using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class GameOverState : GameState
    {
        private SpriteFont font;

        public GameOverState(Game1 game) : base(game) { }

        public override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/PixelFont");
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game.ChangeState(new MenuState(Game));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.DarkRed);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Game Over! Press Enter to Return to Menu", new Vector2(300, 340), Color.White);
            spriteBatch.End();
        }
    }
}