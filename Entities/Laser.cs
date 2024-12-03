using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class Laser
{
    public Vector2 Position { get; set; }
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
        Position += velocity; // Aktualizujemy pozycję lasera na podstawie prędkości
    }

    public bool IsOffScreen()
    {
        return Position.Y > screenHeight; // Sprawdzanie, czy laser opuścił ekran
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Position, Color.White); // Rysowanie lasera
    }
}
