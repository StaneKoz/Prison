using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public interface IEntity 
{
    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch);
}