using Silk.NET.Input;


namespace BasicGraphicsEngine
{
    public enum UserKeyboardKey
    { 
        NONE,

        Q,
        W,
        E,
        R,
        T,
        Y,
        U,
        I,
        O,
        P,
        A,
        S,
        D,
        F,
        G,
        H,
        J,
        K,
        L,
        Z,
        X,
        C,
        V,
        B,
        N,
        M,

        Number0,
        Number1,
        Number2,
        Number3,
        Number4,
        Number5,
        Number6,
        Number7,
        Number8,
        Number9,

        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,

        Space,
        Enter,
        ShiftLeft,
        ShiftRight,
        ControlLeft,
        ControlRight,
        AltLeft,
        AltRight,

        Numpad0,
        Numpad1,
        Numpad2,
        Numpad3,
        Numpad4,
        Numpad5,
        Numpad6,
        Numpad7,
        Numpad8,
        Numpad9
    }

    public enum UserMouseButton
    { 
        NONE,

        Left,
        Middle,
        Right,
    }

    public class UserInput
    {
        private IKeyboard _keyboard;
        private IMouse _mouse;

        internal UserInput(IInputContext input)
        { 
            _keyboard = input.Keyboards[0];
            _mouse = input.Mice[0];
        }

        private Key UserKeyToSilkKey(UserKeyboardKey key)
        {
            switch (key)
            {
                case UserKeyboardKey.Q: return Key.Q;
                case UserKeyboardKey.W: return Key.W;
                case UserKeyboardKey.E: return Key.E;
                case UserKeyboardKey.R: return Key.R;
                case UserKeyboardKey.T: return Key.T;
                case UserKeyboardKey.Y: return Key.Y;
                case UserKeyboardKey.U: return Key.U;
                case UserKeyboardKey.I: return Key.I;
                case UserKeyboardKey.O: return Key.O;
                case UserKeyboardKey.P: return Key.P;
                case UserKeyboardKey.A: return Key.A;
                case UserKeyboardKey.S: return Key.S;
                case UserKeyboardKey.D: return Key.D;
                case UserKeyboardKey.F: return Key.F;
                case UserKeyboardKey.G: return Key.G;
                case UserKeyboardKey.H: return Key.H;
                case UserKeyboardKey.J: return Key.J;
                case UserKeyboardKey.K: return Key.K;
                case UserKeyboardKey.L: return Key.L;
                case UserKeyboardKey.Z: return Key.Z;
                case UserKeyboardKey.X: return Key.X;
                case UserKeyboardKey.C: return Key.C;
                case UserKeyboardKey.V: return Key.V;
                case UserKeyboardKey.B: return Key.B;
                case UserKeyboardKey.N: return Key.N;
                case UserKeyboardKey.M: return Key.M;
                     
                case UserKeyboardKey.Number0: return Key.Number0;
                case UserKeyboardKey.Number1: return Key.Number1;
                case UserKeyboardKey.Number2: return Key.Number2;
                case UserKeyboardKey.Number3: return Key.Number3;
                case UserKeyboardKey.Number4: return Key.Number4;
                case UserKeyboardKey.Number5: return Key.Number5;
                case UserKeyboardKey.Number6: return Key.Number6;
                case UserKeyboardKey.Number7: return Key.Number7;
                case UserKeyboardKey.Number8: return Key.Number8;
                case UserKeyboardKey.Number9: return Key.Number9;
                     
                case UserKeyboardKey.F1: return Key.F1;
                case UserKeyboardKey.F2: return Key.F2;
                case UserKeyboardKey.F3: return Key.F3;
                case UserKeyboardKey.F4: return Key.F4;
                case UserKeyboardKey.F5: return Key.F5;
                case UserKeyboardKey.F6: return Key.F6;
                case UserKeyboardKey.F7: return Key.F7;
                case UserKeyboardKey.F8: return Key.F8;
                case UserKeyboardKey.F9: return Key.F9;
                case UserKeyboardKey.F10: return Key.F10;
                case UserKeyboardKey.F11: return Key.F11;
                case UserKeyboardKey.F12: return Key.F12;
                     
                case UserKeyboardKey.Space: return Key.Space;
                case UserKeyboardKey.Enter: return Key.Enter;
                case UserKeyboardKey.ShiftLeft: return Key.ShiftLeft;
                case UserKeyboardKey.ShiftRight: return Key.ShiftRight;
                case UserKeyboardKey.ControlLeft: return Key.ControlLeft;
                case UserKeyboardKey.ControlRight: return Key.ControlRight;
                case UserKeyboardKey.AltLeft: return Key.AltLeft;
                case UserKeyboardKey.AltRight: return Key.AltRight;
                     
                case UserKeyboardKey.Numpad0: return Key.Keypad0;
                case UserKeyboardKey.Numpad1: return Key.Keypad1;
                case UserKeyboardKey.Numpad2: return Key.Keypad2;
                case UserKeyboardKey.Numpad3: return Key.Keypad3;
                case UserKeyboardKey.Numpad4: return Key.Keypad4;
                case UserKeyboardKey.Numpad5: return Key.Keypad5;
                case UserKeyboardKey.Numpad6: return Key.Keypad6;
                case UserKeyboardKey.Numpad7: return Key.Keypad7;
                case UserKeyboardKey.Numpad8: return Key.Keypad8;
                case UserKeyboardKey.Numpad9: return Key.Keypad9;
            }

            return Key.Tab;
        }

        internal UserKeyboardKey SilkKeyToUserKey(Key key)
        {
            switch (key)
            {
                case Key.Q: return UserKeyboardKey.Q;
                case Key.W: return UserKeyboardKey.W;
                case Key.E: return UserKeyboardKey.E;
                case Key.R: return UserKeyboardKey.R;
                case Key.T: return UserKeyboardKey.T;
                case Key.Y: return UserKeyboardKey.Y;
                case Key.U: return UserKeyboardKey.U;
                case Key.I: return UserKeyboardKey.I;
                case Key.O: return UserKeyboardKey.O;
                case Key.P: return UserKeyboardKey.P;
                case Key.A: return UserKeyboardKey.A;
                case Key.S: return UserKeyboardKey.S;
                case Key.D: return UserKeyboardKey.D;
                case Key.F: return UserKeyboardKey.F;
                case Key.G: return UserKeyboardKey.G;
                case Key.H: return UserKeyboardKey.H;
                case Key.J: return UserKeyboardKey.J;
                case Key.K: return UserKeyboardKey.K;
                case Key.L: return UserKeyboardKey.L;
                case Key.Z: return UserKeyboardKey.Z;
                case Key.X: return UserKeyboardKey.X;
                case Key.C: return UserKeyboardKey.C;
                case Key.V: return UserKeyboardKey.V;
                case Key.B: return UserKeyboardKey.B;
                case Key.N: return UserKeyboardKey.N;
                case Key.M: return UserKeyboardKey.M;

                case Key.Number0: return UserKeyboardKey.Number0;
                case Key.Number1: return UserKeyboardKey.Number1;
                case Key.Number2: return UserKeyboardKey.Number2;
                case Key.Number3: return UserKeyboardKey.Number3;
                case Key.Number4: return UserKeyboardKey.Number4;
                case Key.Number5: return UserKeyboardKey.Number5;
                case Key.Number6: return UserKeyboardKey.Number6;
                case Key.Number7: return UserKeyboardKey.Number7;
                case Key.Number8: return UserKeyboardKey.Number8;
                case Key.Number9: return UserKeyboardKey.Number9;

                case Key.F1: return UserKeyboardKey.F1;
                case Key.F2: return UserKeyboardKey.F2;
                case Key.F3: return UserKeyboardKey.F3;
                case Key.F4: return UserKeyboardKey.F4;
                case Key.F5: return UserKeyboardKey.F5;
                case Key.F6: return UserKeyboardKey.F6;
                case Key.F7: return UserKeyboardKey.F7;
                case Key.F8: return UserKeyboardKey.F8;
                case Key.F9: return UserKeyboardKey.F9;
                case Key.F10: return UserKeyboardKey.F10;
                case Key.F11: return UserKeyboardKey.F11;
                case Key.F12: return UserKeyboardKey.F12;

                case Key.Space: return UserKeyboardKey.Space;
                case Key.Enter: return UserKeyboardKey.Enter;
                case Key.ShiftLeft: return UserKeyboardKey.ShiftLeft;
                case Key.ShiftRight: return UserKeyboardKey.ShiftRight;
                case Key.ControlLeft: return UserKeyboardKey.ControlLeft;
                case Key.ControlRight: return UserKeyboardKey.ControlRight;
                case Key.AltLeft: return UserKeyboardKey.AltLeft;
                case Key.AltRight: return UserKeyboardKey.AltRight;

                case Key.Keypad0: return UserKeyboardKey.Numpad0;
                case Key.Keypad1: return UserKeyboardKey.Numpad1;
                case Key.Keypad2: return UserKeyboardKey.Numpad2;
                case Key.Keypad3: return UserKeyboardKey.Numpad3;
                case Key.Keypad4: return UserKeyboardKey.Numpad4;
                case Key.Keypad5: return UserKeyboardKey.Numpad5;
                case Key.Keypad6: return UserKeyboardKey.Numpad6;
                case Key.Keypad7: return UserKeyboardKey.Numpad7;
                case Key.Keypad8: return UserKeyboardKey.Numpad8;
                case Key.Keypad9: return UserKeyboardKey.Numpad9;
            }

            return UserKeyboardKey.NONE;
        }

        public bool IsKeyPressed(UserKeyboardKey key)
        {
            return _keyboard.IsKeyPressed(UserKeyToSilkKey(key));
        }

        private MouseButton UserMouseButtonToSilkMouseButton(UserMouseButton button)
        {
            switch (button)
            {
                case UserMouseButton.Left: return MouseButton.Left;
                case UserMouseButton.Middle: return MouseButton.Middle;
                case UserMouseButton.Right: return MouseButton.Right;
            }

            return MouseButton.Middle;
        }

        internal UserMouseButton SilkMouseButtonToUserMouseButton(MouseButton button)
        {
            switch (button)
            { 
                case MouseButton.Left: return UserMouseButton.Left;
                case MouseButton.Middle: return UserMouseButton.Middle;
                case MouseButton.Right: return UserMouseButton.Right;
            }

            return UserMouseButton.NONE;
        }

        public bool IsMouseButtonPressed(UserMouseButton button)
        {
            return _mouse.IsButtonPressed(UserMouseButtonToSilkMouseButton(button));
        }
    }
}
