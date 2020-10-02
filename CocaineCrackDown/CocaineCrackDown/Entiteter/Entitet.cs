using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CocaineCrackDown.Entiteter {
    public abstract class Entitet : IEntitet {
        protected SpriteBatch spriteBatch;
        protected Texture2D texture;
        protected Vector2 position;
        protected ContentManager content;
        protected KeyboardState currentKeyboardState;
        protected KeyboardState previousKeyboardState;
        protected Color color;
        public Input input;
        protected Entitet(SpriteBatch sp,ContentManager cm,Color c) {
            spriteBatch = sp;
            content = cm;
            color = c;
        }
        public abstract void Initialize();
        public abstract void AddedToScene(Vector2 v2);
        public abstract void Update(GameTime gameTime);
        public abstract void RemovedFromScene();
        public abstract void Draw(GameTime gameTime);

    }

}
