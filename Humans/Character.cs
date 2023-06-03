using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Prison.Humans;
using Prison.Levels;
using Prison.Managers;

namespace Prison.Entities
{
    public abstract class Character : IEntity, ICollisionActor
    {
        public const float Gravity = 900f;
        protected int Health;
        public Vector2 Position { get; protected set; }
        protected Vector2 ShiftVector;

        public Vector2 Velocity;
        protected float VelocityXCoefficient = 1f;
        public float SpeedX = 200;
        public float WalkSpeedCoefficient = 0.5f;
        public Level Level;
        protected float JumpSpeed = 400;

        public bool IsOnGround { get; set; }
        public IShapeF Bounds => new RectangleF(Position.X + ShiftVector.X,
            Position.Y + ShiftVector.Y, CollisionRectangleSize.Width, CollisionRectangleSize.Height);
        public Size2 CollisionRectangleSize;
        public AnimationManager AnimationManager = new();
        protected CharacterState State;
        private bool isTurnRight;
        public bool WalkToggle;
        public bool IsTurnRight => isTurnRight = Velocity.X > 0 || Velocity.X == 0 && isTurnRight;

        
        public void Jump()
        {
            if (IsOnGround)
                Velocity.Y -= JumpSpeed;
        }

        public void MoveX(bool isRight)
        {
            Velocity.X += SpeedX * VelocityXCoefficient * (isRight ? 1 : -1);
        }

        public virtual void SwitchDefaultState()
        {
            if (WalkToggle) SwitchWalkToggle();
        }

        public void SwitchWalkToggle()
        {
            if (WalkToggle)
            {
                VelocityXCoefficient /= WalkSpeedCoefficient;
                Velocity /= WalkSpeedCoefficient;
                WalkToggle = false;
            }
            else
            {
                SwitchDefaultState();
                VelocityXCoefficient *= WalkSpeedCoefficient;
                Velocity *= WalkSpeedCoefficient;
                WalkToggle = true;
            }
        }

        public void StopX(bool isRight) => MoveX(!isRight);

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void LoadContent();
        public abstract void OnCollision(CollisionEventArgs collisionInfo);
    }
}
