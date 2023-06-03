using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Timers;
using Prison.Collision;
using Prison.Entities;
using Prison.Humans;
using Prison.Levels;
using System;

namespace Prison.Collision
{
    internal class FOV : ICollisionActor
    {
        private Enemy enemy;
        private RectangleF collisionRectangle;
        private Size2 defaultCollisionSize;
        private Color color;
        public IShapeF Bounds => collisionRectangle;

        public FOV(float width, float height, Vector2 position, Enemy enemy)
        {
            collisionRectangle = new RectangleF(position.X, position.Y, width, height);
            defaultCollisionSize = new Size2(width, height);
            this.enemy = enemy;
            color = Color.Black;
        }

        public void Update(GameTime gameTime)
        {
            collisionRectangle.X = enemy.Bounds.Position.X + enemy.CollisionRectangleSize.Width / 2;
            if (enemy.IsTurnRight)
            {
                collisionRectangle.Width = defaultCollisionSize.Width;
            }
            else
            {
                collisionRectangle.X -= defaultCollisionSize.Width;
            }
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            var other = collisionInfo.Other;
            if (other is Wall wall)
            {
                if (collisionInfo.PenetrationVector.X > 0)
                {
                    collisionRectangle.Width = wall.Bounds.Position.X - collisionRectangle.Position.X;
                }
                else
                {
                    var delta = collisionRectangle.X - wall.Bounds.Position.X + ((RectangleF)wall.Bounds).Width;
                    collisionRectangle = new RectangleF(collisionRectangle.Position - new Vector2(delta, 0), new Size2(delta, collisionRectangle.Size.Height));
                }
            }
            else if (other is Player)
            {
                enemy.enemyState = EnemyState.Chase;
                enemy.LastPlayerPosition = other.Bounds.Position;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(collisionRectangle, enemy.enemyState == EnemyState.Chase ? Color.Red : Color.Black);
        }
    }
}
