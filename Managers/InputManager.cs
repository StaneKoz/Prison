
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using Prison.Entities;

namespace Prison.Managers
{
    internal class InputManager
    {
        private KeyboardListener keyboardListener = new(new() { RepeatPress = false });
        private Player player;

        public InputManager(Player player)
        {
            keyboardListener.KeyPressed += OnControlPressed;
            keyboardListener.KeyReleased += OnControlReleased;
            this.player = player;
        }

        public void OnControlPressed(object sender, KeyboardEventArgs args) 
        {
            switch (args.Key)
            {
                case Keys.A:
                    player.MoveX(false);
                    break;
                case Keys.D:
                    player.MoveX(true);
                    break;
                case Keys.Space:
                    player.Jump();
                    break;
                case Keys.LeftControl:
                    player.SwitchCrouchToggle();
                    break;
                case Keys.R:
                    player.SwitchWalkToggle();
                    break;
            }
        }

        public void OnControlReleased(object sender, KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Keys.A:
                    player.StopX(false);
                    break;
                case Keys.D:
                    player.StopX(true);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            keyboardListener.Update(gameTime);
        }
    }
}
