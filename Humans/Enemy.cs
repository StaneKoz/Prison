
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Prison.Collision;
using Prison.Graphic;
using Prison.Algorithms;
using Prison.Collision;
using Prison.Humans;
using Prison.Levels;
using Prison.Managers;
using System;

namespace Prison.Entities
{
    internal class Enemy : Character
    {
        private EnemyManager enemyManager;
        public EnemyState enemyState = EnemyState.Patrol;
        public Vector2 LastPlayerPosition;
        public FOV[] FOVs;

        public Enemy(Vector2 startPatrolPoint, Vector2 endPatrolPoint, Size2 collisionRectangleSize, Vector2 shiftVector, Level level, int countFOV)
        {
            this.Level = level;
            CollisionRectangleSize = collisionRectangleSize;
            ShiftVector = shiftVector;
            Position = startPatrolPoint;
            enemyManager = new(this, startPatrolPoint, endPatrolPoint);
            SpeedX = 200;
            WalkSpeedCoefficient = 70f / 200f;
            var v = new Vector2();
            FOVs = new FOV[countFOV];
            for (int i = 0; i < countFOV; i++)
            {
                var fov = new FOV(200, 10, Bounds.Position with { X = Bounds.Position.X + CollisionRectangleSize.Width / 2,
                    Y = Bounds.Position.Y + 10 + i * 10}, this);
                FOVs[i] = fov;
                level.viewComponent.Insert(fov);
                level.collisionComponent.Insert(fov);
            }
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            var other = collisionInfo.Other;
            if (other is Wall wall)
            {
                if (collisionInfo.PenetrationVector.Y != 0)
                {
                    IsOnGround = collisionInfo.PenetrationVector.Y > 0;
                    Velocity.Y = 0;
                }
                Position -= collisionInfo.PenetrationVector;
            }
        }

        public override void Update(GameTime gameTime)
        {
            enemyManager.Update(gameTime);
            Velocity.Y += Gravity * gameTime.GetElapsedSeconds();
            Position += Velocity * gameTime.GetElapsedSeconds();
            if (IsOnGround)
            {
                if (Velocity.X == 0)
                    State = CharacterState.Idle;
                else
                    State = WalkToggle ? CharacterState.Walk : CharacterState.Run;
            }
            else
            {
                State = Velocity.Y >= 0 ? CharacterState.Fall : CharacterState.Jump;
            }
            IsOnGround = false;
            enemyState = EnemyState.Patrol;
            AnimationManager.Update(State, gameTime, IsTurnRight);
            foreach (var fov in FOVs)
            {
                fov.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Draw(spriteBatch, Position, Color.Red);
            foreach (var fov in FOVs)
            {
                fov.Draw(spriteBatch);
            }
            
        }
        public override void LoadContent()
        {
            AnimationManager.AddAnimation(CharacterState.Idle,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_Idle"), 10, 1, 1 / 10f));
            AnimationManager.AddAnimation(CharacterState.Walk,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_Run"), 10, 1, 1 / 7f));
            AnimationManager.AddAnimation(CharacterState.Run,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_Run"), 10, 1, 1 / 10f));
            AnimationManager.AddAnimation(CharacterState.Jump,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_Jump"), 3, 1, 1 / 10f));
            AnimationManager.AddAnimation(CharacterState.Fall,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_Fall"), 3, 1, 1 / 10f));
            AnimationManager.AddAnimation(CharacterState.CrouchIdle,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_CrouchFull"), 3, 1, 1 / 3f));
            AnimationManager.AddAnimation(CharacterState.CrouchWalk,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_CrouchWalk"), 8, 1, 1 / 8f));
        }
    }
}
