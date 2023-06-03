
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Threading;

namespace Prison.Graphic
{
    public class Animation
    {
        private readonly Texture2D _texture;
        private readonly List<Rectangle> _sourceRectangles = new();
        public Size2 SizeAnimationFrame;    
        private readonly int _framesCount; 
        private int _frameIndex;
        private readonly float _frameTime;
        private float _frameTimeLeft;
        private bool _isPlaying = true;
        private SpriteEffects _spriteEffects;
        public Animation(Texture2D texture, int framesCountX, int framesCountY, float frameTime)
        {
            _texture = texture;
            _frameTime = frameTime;
            _frameTimeLeft = frameTime;
            _framesCount = framesCountX * framesCountY;
            var frameWidth = _texture.Width / framesCountX;
            var frameHeight = _texture.Height / framesCountY;
            SizeAnimationFrame = new Size2 (frameWidth, frameHeight);
            for (int i = 0; i < framesCountY; i++)
            {
                for (int j = 0; j < framesCountX; j++)
                {
                    _sourceRectangles.Add(new(j * frameWidth, i * frameHeight, frameWidth, frameHeight));
                }
            }
        }

        public void Start()
        {
            _isPlaying = true;
        }

        public void Stop()
        {
            _isPlaying = false;
        }

        public void Reset()
        {
            _frameIndex = 0;
            _frameTimeLeft = _frameTime;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isPlaying) return;
            _frameTimeLeft -= gameTime.GetElapsedSeconds();
            if (_frameTimeLeft <= 0 ) 
            {
                _frameTimeLeft += _frameTime;
                _frameIndex = (_frameIndex + 1) % _framesCount;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, Color color)
        {
            spriteBatch.Draw(_texture, 
                position, 
                _sourceRectangles[_frameIndex],
                color,
                0, 
                Vector2.Zero, 
                Vector2.One,
                spriteEffects, 
                1);
        }
    }
}
