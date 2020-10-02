
using Microsoft.Xna.Framework;

namespace CocaineCrackDown.Entiteter {
    public interface IEntitet {
        void Initialize();
        void AddedToScene(Vector2 v2);
        void Update(GameTime gameTime);
        void RemovedFromScene();
        void Draw(GameTime gameTime);

    }
}
