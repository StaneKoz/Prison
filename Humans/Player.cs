using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Prison.Collision;
using Prison.Graphic;
using Prison.Humans;
using Prison.Levels;
using System;

namespace Prison.Entities
{
    public class Player : Character
    {
        public bool CrouchToggle;

        public Player(Size2 collisionRectangleSize, Vector2 shiftVector) 
        {
            CollisionRectangleSize = collisionRectangleSize;
            ShiftVector = shiftVector;
            SpeedX = 200;
        }

        public override void SwitchDefaultState()
        {
            base.SwitchDefaultState();
            if (CrouchToggle) SwitchCrouchToggle();
        }

        public void SwitchCrouchToggle()
        {
            if (CrouchToggle)
            {
                VelocityXCoefficient /= WalkSpeedCoefficient;
                Velocity /= WalkSpeedCoefficient;
                CrouchToggle = false;
            }
            else
            {
                SwitchDefaultState();
                VelocityXCoefficient *= WalkSpeedCoefficient;
                Velocity *= WalkSpeedCoefficient;
                CrouchToggle = true;
            }
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.Other is Wall wall)
            {
                if (collisionInfo.PenetrationVector.Y != 0)
                {
                    IsOnGround = collisionInfo.PenetrationVector.Y > 0;
                    Velocity.Y = 0;
                }
                Position -= collisionInfo.PenetrationVector;
            }
        }

        public override void LoadContent()
        {
            AnimationManager.AddAnimation(CharacterState.Idle,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_Idle"), 10, 1, 1 / 10f));
            AnimationManager.AddAnimation(CharacterState.Walk,
                new Animation(Global.ContentManager.Load<Texture2D>("Characters/Knight/_Run"), 10, 1, 1 / 10f));
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

        public override void Update(GameTime gameTime)
        {
            Velocity.Y += Gravity * gameTime.GetElapsedSeconds();
            //if (Velocity.X != 0)
            //{
            //    Velocity.X = CrouchToggle || WalkToggle ? WalkSpeed : Velocity.X;
            //}
            Position += Velocity * gameTime.GetElapsedSeconds();
            if (IsOnGround)
            {
                if (Velocity.X == 0)
                    State = CrouchToggle ? CharacterState.CrouchIdle : CharacterState.Idle;
                else
                    State = CrouchToggle ? CharacterState.CrouchWalk : WalkToggle ? CharacterState.Walk : CharacterState.Run;
            }
            else
            {
                State = Velocity.Y >= 0 ? CharacterState.Fall : CharacterState.Jump;
            }
            IsOnGround = false;
            //PreviousVelocity = Velocity;
            AnimationManager.Update(State, gameTime, IsTurnRight);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationManager.Draw(spriteBatch, Position, Color.LightGreen);
        }
    }
}
