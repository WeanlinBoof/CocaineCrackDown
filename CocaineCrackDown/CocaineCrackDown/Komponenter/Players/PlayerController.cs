using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework.Input;

using Nez;

namespace CocaineCrackDown.Komponenter {
    public class PlayerController : Component, IUpdatable {
        private VirtualButton _exitGameButton;
        private VirtualJoystick _leftStick;
        private VirtualJoystick _rightStick;
        private VirtualButton _fireButton;
        private VirtualButton _reloadButton;
        private VirtualButton _dropGunButton;
        private VirtualButton _debugButton;
        private VirtualButton _interactButton;
        private VirtualButton _switchWeaponButton;
        private VirtualButton _sprintButton;
        //private VirtualMouseJoystick _mouseJoystick;

        private Spelare spelare;

        private GamePadData Kontroller;
        private bool _inputEnabled = true;

        public bool ExitGameButtonPressed => _exitGameButton?.IsPressed ?? false;
        public float XLeftAxis => _leftStick?.Value.X ?? Kontroller.GetLeftStick().X;
        public float YLeftAxis => -1 * _leftStick?.Value.Y ?? -1 * Kontroller.GetLeftStick().Y;
        public float XRightAxis => _rightStick?.Value.X ?? Kontroller.GetRightStick().X;
        public float YRightAxis => -1 * _rightStick?.Value.Y ?? -1 * Kontroller.GetRightStick().Y;
        public bool FirePressed => _fireButton?.IsDown ?? Kontroller.IsButtonDown(Buttons.RightTrigger);
        public bool ReloadPressed => _reloadButton?.IsPressed ?? Kontroller.IsButtonPressed(Buttons.X);
        public bool DropWeaponPressed => _dropGunButton?.IsPressed ?? Kontroller.IsButtonPressed(Buttons.DPadUp);
        public bool InteractPressed => _interactButton?.IsPressed ?? Kontroller.IsButtonPressed(Buttons.A);
        public bool DebugModePressed => _debugButton?.IsPressed ?? Kontroller.IsButtonPressed(Buttons.Start);
        public bool SwitchWeaponPressed => _switchWeaponButton?.IsPressed ?? Kontroller.IsButtonPressed(Buttons.Y);
        public bool SprintPressed => _sprintButton?.IsPressed ?? Kontroller.IsButtonPressed(Buttons.LeftShoulder);
        public bool SprintDown => _sprintButton?.IsDown ?? Kontroller.IsButtonDown(Buttons.LeftShoulder);
        public bool InputEnabled => _inputEnabled;

        public PlayerController(GamePadData kontrollerData) {
            Kontroller = kontrollerData;
        }

        public override void OnAddedToEntity() {
            spelare = Entity as Spelare;

            // Keyboard player
            if(Kontroller == null) {
                // Virtual mouse joystick
                //_mouseJoystick = new VirtualMouseJoystick(spelare.position);

                // Virtual joysticks
                _leftStick = new VirtualJoystick(false);
                _rightStick = new VirtualJoystick(true);

                // Buttons
                _exitGameButton = new VirtualButton();
                _fireButton = new VirtualButton();
                _reloadButton = new VirtualButton();
                _dropGunButton = new VirtualButton();
                _debugButton = new VirtualButton();
                _interactButton = new VirtualButton();
                _switchWeaponButton = new VirtualButton();
                _sprintButton = new VirtualButton();

                _exitGameButton.AddKeyboardKey(Keys.Escape);
                _leftStick.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut , Keys.A , Keys.D , Keys.S , Keys.W);
                //_rightStick.Nodes.Add(_mouseJoystick);
                _fireButton.AddMouseLeftButton();
                _reloadButton.AddKeyboardKey(Keys.R);
                _interactButton.AddKeyboardKey(Keys.E);
                _dropGunButton.AddKeyboardKey(Keys.G);
                _debugButton.AddKeyboardKey(Keys.F2);
                _switchWeaponButton.AddKeyboardKey(Keys.Q);
                _sprintButton.AddKeyboardKey(Keys.LeftShift);
            }
        }

        public void SetInputEnabled(bool enabled) {
            _inputEnabled = enabled;
        }

        public void Update() { 
            //if(_gp == null) {
            //    _mouseJoystick.ReferencePoint = entity.scene.camera.worldToScreenPoint(spelare.position);
            //}
        }
    }
}