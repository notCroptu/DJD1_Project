using UnityEngine;
using InControl;

public class PlayerActions : MonoBehaviour
{
    private InputDevice inputDevice;
    public InputControl Jump { get; private set; }
    public InputControl Move { get; private set; }
    public InputControl ShapeshiftX { get; private set; }
    public InputControl ShapeshiftY { get; private set; }
    public InputControl Ability { get; private set; }
    public InputControl AimX { get; private set; }
    public InputControl AimY { get; private set; }

    void Start()
    {
        inputDevice = InputManager.ActiveDevice;

        Jump = inputDevice.Action1;
        Move = inputDevice.LeftStickX;
        ShapeshiftX = inputDevice.RightStickX;
        ShapeshiftY = inputDevice.RightStickY;
        Ability = inputDevice.RightTrigger;
        AimX = inputDevice.RightStickX;
        AimY = inputDevice.RightStickY;
    }
}
