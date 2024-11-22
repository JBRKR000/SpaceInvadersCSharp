using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class PowerUp
{
    private Texture2D texture;
    private Vector2 position;
    public bool IsCollected { get; private set; }
    public Rectangle Rectangle { get; private set; }

    public PowerUp(Texture2D texture, Vector2 position)
    {
        this.texture = texture;
        this.position = position;
        IsCollected = false;
        Rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
    }

    public void Update(GameTime gameTime)
    {
        position.Y += 150f * (float)gameTime.ElapsedGameTime.TotalSeconds; // Ruch w dół
        Rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
    }

    public void Collect()
    {
        IsCollected = true;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsCollected)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
