using Riptide.Demos.PlayerHosted;
using Shared;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;
    private float _timeSinceLastUpdate;


    void Update()
    {
        _timeSinceLastUpdate += Time.deltaTime;

        if (_timeSinceLastUpdate > NetworkConfig.TickInterval)
        {
            _timeSinceLastUpdate = 0;
            SendInput();
        }
    }

    private void SendInput()
    {
        PlayerKeyboard keyboard = default;
        keyboard |= Input.GetKey(KeyCode.W) ? PlayerKeyboard.Up : 0;
        keyboard |= Input.GetKey(KeyCode.S) ? PlayerKeyboard.Down : 0;
        keyboard |= Input.GetKey(KeyCode.A) ? PlayerKeyboard.Left : 0;
        keyboard |= Input.GetKey(KeyCode.D) ? PlayerKeyboard.Right : 0;
        keyboard |= Input.GetKey(KeyCode.Alpha1) || Input.GetMouseButton(0) ? PlayerKeyboard.AttackSkill1 : 0;
        keyboard |= Input.GetKey(KeyCode.Alpha2) || Input.GetMouseButton(1) ? PlayerKeyboard.AttackSkill2 : 0;

        var playerInput = new PlayerInput
        {
            Keyboard = keyboard
        };
        _networkManager.Send(new InputRequest
        {
            Input = playerInput
        }, GameMessageId.InputRequest, true);
    }
}