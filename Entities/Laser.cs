using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceInvaders;

public class Laser
{
	private Vector2 position;
	private Texture2D laserTexture;
	internal Rectangle rectangle;
	public int damage = 10;
	public Enemy1 owner;
	private float timeAlive; // Czas życia lasera
	private float targetCenterX; // Środek bossa, za którym laser ma podążać
	private float followSpeed = 5f; // Prędkość, z jaką laser podąża za środkiem bossa

	public Laser(Texture2D laserTexture, Vector2 startPosition, Enemy1 owner, int damage, float targetCenterX)
	{
		this.position = startPosition;
		this.laserTexture = laserTexture;
		this.owner = owner;
		this.damage = damage;
		this.rectangle = new Rectangle((int)position.X, (int)position.Y, laserTexture.Width, laserTexture.Height);
		this.timeAlive = 0f; // Inicjalizujemy czas życia
		this.targetCenterX = targetCenterX; // Ustawiamy środek bossa jako punkt docelowy dla lasera
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(laserTexture, position, Color.White);
	}

	public void Update(GameTime gameTime)
	{
		// Aktualizacja czasu życia
		timeAlive += (float)gameTime.ElapsedGameTime.TotalSeconds;

		// Laser podąża za środkiem bossa
		position.X += (targetCenterX - position.X) * followSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

		// Aktualizacja prostokąta reprezentującego laser
		rectangle = new Rectangle((int)position.X, (int)position.Y, laserTexture.Width, laserTexture.Height);
	}

	public bool IsExpired()
	{
		return timeAlive > 3f; // Laser znika po 3 sekundach
	}

	// Metoda do aktualizacji docelowego środka bossa w przypadku zmiany pozycji
	public void UpdateTargetCenter(float newTargetCenterX)
	{
		targetCenterX = newTargetCenterX;
	}
}
