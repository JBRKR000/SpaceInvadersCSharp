using System;
using Microsoft.Xna.Framework;
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

    public Boss(Texture2D texture, Vector2 position, float bossSpeed, Game1 game)
    {
        this.bossTexture = texture;
        this.Game = game;
        this.position = position;
        this.bossSpeed = bossSpeed;
        this.Game = game;
        this.maxHealth = 2000;
        this.health = maxHealth;
        
    }

    public void Update(GameTime gameTime)
    {
        random = new Random();
        rectangle = new Rectangle((int)position.X, (int)position.Y, bossTexture.Width, bossTexture.Height);
        timeSinceLastDirectionChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
}