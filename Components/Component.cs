﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Components
{
	internal abstract class Component
	{
		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void Update(GameTime gameTime);

	}
}
