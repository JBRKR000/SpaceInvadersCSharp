using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public enum PowerUpType
{
	Health,
	FireRate,
	Shield
}

public class PowerUp
{
	private Texture2D powerupTexture;
	private Vector2 position;
	public bool IsCollected { get; private set; }
	public Rectangle Rectangle { get; private set; }
	public PowerUpType Type { get; private set; } // Typ power-upa

	public PowerUp(Texture2D texture, Vector2 position, PowerUpType type)
	{
		this.powerupTexture = texture;
		this.position = position;
		this.Type = type;
		IsCollected = false;
		Rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
	}

	public void Update(GameTime gameTime)
	{
		position.Y += 150f * (float)gameTime.ElapsedGameTime.TotalSeconds; // Ruch w dół
		Rectangle = new Rectangle((int)position.X, (int)position.Y, powerupTexture.Width, powerupTexture.Height);
	}

	public void Collect()
	{
		IsCollected = true;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		if (!IsCollected)
		{
			spriteBatch.Draw(powerupTexture, position, Color.White);
		}
	}
}
