using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static CocaineCrackDown.Constants;

namespace CocaineCrackDown.Entiteter {
    public class Spelare : Entitet {
        
        public Spelare(SpriteBatch sp, ContentManager cm,Color c) : base(sp,cm,c){
            input = new Input();
        }
        public override void Initialize() {
        }
        public override void AddedToScene(Vector2 v2) {
            texture = content.Load<Texture2D>(stilla1);
            position = v2;
        }

        public override void Update(GameTime gameTime) {

            currentKeyboardState = Keyboard.GetState();

            if(currentKeyboardState.IsKeyDown(input.Up)) {
                position.Y--;
            }
            if(currentKeyboardState.IsKeyDown(input.Down)) {
                position.Y++;
            }
            if(currentKeyboardState.IsKeyDown(input.Right)) {
                position.X++;
            }
            if(currentKeyboardState.IsKeyDown(input.Left)) {
                position.X--;
            }
            if(currentKeyboardState.IsKeyDown(input.Attack)) {

            }

        }
        public override void RemovedFromScene() {
        }
        public override void Draw(GameTime gameTime) {
            spriteBatch.Draw(texture,position,color);
        }
    }

}
