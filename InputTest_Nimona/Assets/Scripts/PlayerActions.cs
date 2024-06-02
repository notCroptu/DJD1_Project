using UnityEngine;
using InControl;

public class PlayerActions : MonoBehaviour
{
    private InputDevice inputDevice;
    public InputControl Jump { get; private set; }
    public InputControl MoveX { get; private set; }
    public InputControl MoveY { get; private set; }
    public InputControl ShapeshiftX { get; private set; }
    public InputControl ShapeshiftY { get; private set; }
    public InputControl Ability { get; private set; }
    public InputControl Throw { get; private set; }
    public InputControl AimX { get; private set; }
    public InputControl AimY { get; private set; }

    void Start()
    {
        inputDevice = InputManager.ActiveDevice;

        Jump = inputDevice.Action1;
        MoveX = inputDevice.LeftStickX;
        MoveY = inputDevice.LeftStickY;
        ShapeshiftX = inputDevice.RightStickX;
        ShapeshiftY = inputDevice.RightStickY;
        Ability = inputDevice.RightTrigger;
        Throw = inputDevice.RightStickButton;
        AimX = inputDevice.RightStickX;
        AimY = inputDevice.RightStickY;
    }
}
