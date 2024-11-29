using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders;

public class Boss
{
    protected Texture2D bossTexture;
    protected Vector2 position;
    protected float bossSpeed;
    public Rectangle rectangle;
    protected Game1 Game;
    public int health;
    protected Double directionChangeInterval = new Random().NextDouble();
    protected float timeSinceLastDirectionChange = 0f;
    protected Random random;
    protected int direction = 1;
    protected int maxHealth;
    protected Color healthBarColor = Color.Green;
    protected Color healthBarBackgroundColor = Color.Red;
    private int directionX;
    private int directionY;

	protected SoundEffect bulletSound = SoundEffect.FromFile("../../../Content/Sounds/2.wav");
	protected SoundEffectInstance bulletSoundInstance;

	private Texture2D bulletTexture;
	protected List<EnemyBullet> bullets;
	protected double timeSinceLastShot = 0f;
    public int score;

	public Boss(Texture2D texture, Vector2 position, float bossSpeed, Game1 game, List<EnemyBullet> bullets, Texture2D bulletTexture, int score)
    {
        this.bossTexture = texture;
        this.Game = game;
        this.position = position;
        this.bossSpeed = bossSpeed;
        this.maxHealth = 2000;
        this.health = maxHealth;
		this.bullets = bullets;
		this.bulletTexture = bulletTexture;
		bulletSoundInstance = bulletSound.CreateInstance();
        this.score = score;

	}

	public void Update(GameTime gameTime)
    {
        


		Random random = new Random();
		rectangle = new Rectangle((int)position.X, (int)position.Y, bossTexture.Width, bossTexture.Height);
		timeSinceLastDirectionChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

		double randomDelay = (random.NextDouble() * 10 / 2.5 * 12);
		randomDelay = randomDelay < 3 ? randomDelay : (random.NextDouble() * 10 / 2.5 * 10);
		double.Round(randomDelay);

		if (timeSinceLastDirectionChange >= directionChangeInterval)
        {
            directionX = random.Next(-1, 2); 
            directionY = random.Next(-1, 2); 

            timeSinceLastDirectionChange = 0f;
        }
        position.X += directionX * bossSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        position.Y += directionY * bossSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (position.X < 0) position.X = 0;
        if (position.X > Game.GraphicsDevice.Viewport.Width - bossTexture.Width)
            position.X = Game.GraphicsDevice.Viewport.Width - bossTexture.Width;
        if(position.Y < 0) position.Y = 0;
        if (position.Y > Game.GraphicsDevice.Viewport.Height/2 - bossTexture.Height)
            position.Y = Game.GraphicsDevice.Viewport.Height/2 - bossTexture.Height;


		timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
		if (timeSinceLastShot >= randomDelay)
		{
			Shoot();
			timeSinceLastShot = 0f;
		}
	}

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(bossTexture, position, Color.White);
        DrawHealthBar(spriteBatch);
    }
    private void DrawHealthBar(SpriteBatch spriteBatch)
    {
        int barWidth = 50;
        int barHeight = 5;
        int healthBarOffset = 10;
        float healthPercentage = (float)health / maxHealth;
        int healthBarCurrentWidth = Math.Max(1, (int)(barWidth * healthPercentage)); // Avoid zero width
        Vector2 barPosition = new Vector2(position.X + (bossTexture.Width - barWidth) / 2, position.Y + bossTexture.Height + healthBarOffset);
        spriteBatch.Draw(Game1.CreateRectangleTexture(Game.GraphicsDevice, barWidth, barHeight, healthBarBackgroundColor), barPosition, Color.White);
        spriteBatch.Draw(Game1.CreateRectangleTexture(Game.GraphicsDevice, healthBarCurrentWidth, barHeight, healthBarColor), barPosition, Color.White);
    }


	protected virtual void Shoot()
	{
		bulletSoundInstance.Volume = 0.25f;
		if (bulletSoundInstance.State != SoundState.Playing)
		{
			bulletSoundInstance.Play();
		}

		Vector2 bulletPosition1 = new Vector2(position.X + bossTexture.Width / 2, position.Y);
		Vector2 bulletPosition2 = new Vector2(position.X + bossTexture.Width / 2 - 50, position.Y);
		bullets.Add(new EnemyBullet(bulletTexture, bulletPosition1,  10));
		bullets.Add(new EnemyBullet(bulletTexture, bulletPosition2,  10));
	}
}
