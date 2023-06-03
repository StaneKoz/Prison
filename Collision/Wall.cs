using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Prison;
using System;

namespace Prison.Collision
{
    public class Wall : ICollisionActor
    {
        public IShapeF Bounds => CollisionRectangle;
        private RectangleF CollisionRectangle;

        public Wall(Size2 size, Vector2 position)
        {
            CollisionRectangle = new RectangleF(position, size);
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            
        }
    }
}
