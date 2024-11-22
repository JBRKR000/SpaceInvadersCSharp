using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceInvaders.Components;

namespace SpaceInvaders
{
	public class MenuState : GameState
	{
		private SpriteFont font;
		private Texture2D buttonPlayTexture;
		private Texture2D buttonQuitTexture;
		private Texture2D buttonControlsTexture;
		private Texture2D background;
		private Button playButton;
		private Button controlsButton;
		private Button quitButton;
		private SoundEffect music = SoundEffect.FromFile("../../../Content/Music/ChaseTheDestroyers.wav");
		private SoundEffectInstance musicInstance;
		public static bool isMusicMenuPlaying = false;

		public MenuState(Game1 game) : base(game)
		{
			musicInstance = music.CreateInstance();
			musicInstance.Volume = 0.20f;
			musicInstance.IsLooped = true;
		}

		public override void LoadContent()
		{
			font = Game.Content.Load<SpriteFont>("Fonts/PixelFont");
			buttonPlayTexture = Game.Content.Load<Texture2D>("Controls/buttonPlay");
			buttonControlsTexture = Game.Content.Load<Texture2D>("Controls/buttonControls");
			buttonQuitTexture = Game.Content.Load<Texture2D>("Controls/buttonQuit");
			background = Game.Content.Load<Texture2D>("Backgrounds/menuBackground");

			// Inicjalizacja guzików
			playButton = new Button(buttonPlayTexture)
			{
				Position = new Vector2(510, 200),
			};
			playButton.Click += StartButton_Click;

			controlsButton = new Button(buttonControlsTexture)
			{
				Position = new Vector2(510, 300),
			};
			controlsButton.Click += ControlsButton_Click;

			quitButton = new Button(buttonQuitTexture)
			{
				Position = new Vector2(510, 400),
			};
			quitButton.Click += QuitButton_Click;

			// Rozpoczęcie muzyki w menu
			if (!isMusicMenuPlaying)
			{
				musicInstance.Play();
				isMusicMenuPlaying = true;
			}
		}

		private void StartButton_Click(object sender, System.EventArgs e)
		{
			StopMusic();
			Game.ChangeState(new GameplayState(Game));
		}

		private void ControlsButton_Click(object sender, System.EventArgs e)
		{
			StopMusic();
			Game.ChangeState(new ControlsState(Game));
		}

		private void QuitButton_Click(object sender, System.EventArgs e)
		{
			StopMusic();
			Game.Exit();
		}

		private void StopMusic()
		{
			if (isMusicMenuPlaying)
			{
				musicInstance.Stop();
				isMusicMenuPlaying = false;
			}
		}

		public override void Update(GameTime gameTime)
		{
			playButton.Update(gameTime);
			controlsButton.Update(gameTime);
			quitButton.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(background, Game.GraphicsDevice.Viewport.Bounds, Color.White);
			playButton.Draw(spriteBatch);
			controlsButton.Draw(spriteBatch);
			quitButton.Draw(spriteBatch);
			spriteBatch.End();
		}
	}
}
