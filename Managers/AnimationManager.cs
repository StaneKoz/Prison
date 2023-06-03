using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Prison.Graphic;
using Prison.Humans;
using System.Collections.Generic;
using System.Linq;

namespace Prison.Managers
{
    public class AnimationManager
    {
        private readonly Dictionary<CharacterState, Animation> _animations = new();
        private SpriteEffects _spriteEffect;
        private CharacterState _lastKey;
        public Size2 SizeAnimationFrame => _animations[_lastKey].SizeAnimationFrame;

        public AnimationManager()
        {

        }

        public void AddAnimation(CharacterState key, Animation animation)
        {
            _animations.Add(key, animation);
            _lastKey = key;
        }

        public void Update(CharacterState key, GameTime gameTime, bool isMovingRight)
        {
            _spriteEffect = isMovingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (_animations.TryGetValue(key, out Animation value))
            {
                value.Start();
                _animations[key].Update(gameTime);
                _lastKey = key;
            }
            else
            {
                _animations[_lastKey].Stop();
                _animations[_lastKey].Reset();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            _animations[_lastKey].Draw(spriteBatch, position, _spriteEffect, color);
        }
    }
}
