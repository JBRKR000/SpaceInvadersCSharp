using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class Laser
{
    public Vector2 Position { get; set; }
    public Vector2 Position2;
    public Vector2 Position3;
    
    public Rectangle rectangle => new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
    private Texture2D texture;
    private Vector2 velocity;
    public int damage;
    private int screenHeight;

    // Przekazujemy wysokość ekranu do konstruktora
    public Laser(Texture2D texture, Vector2 position, Vector2 velocity)
    {
        this.texture = texture;
        this.Position = position;
        this.velocity = velocity;
        this.damage = 1; // Przykładowe obrażenia
    }

    public void Update(GameTime gameTime)
    {
        Position += velocity;
    }

    public bool IsOffScreen()
    {
        return Position.Y > screenHeight;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Position2.X = 70;
        Position2.Y = 85;
        Position3.X = -17;
        Position3.Y = 85;
        //spriteBatch.Draw(texture, Position + Position2, Color.White);
        spriteBatch.Draw(texture, Position + Position2, Color.White);
        
    }
}
