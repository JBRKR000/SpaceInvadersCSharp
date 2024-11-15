using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders;

public abstract class GameState
{
    protected Game1 Game;

    public GameState(Game1 game)
    {
        Game = game;
    }
	

	public abstract void LoadContent();
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}