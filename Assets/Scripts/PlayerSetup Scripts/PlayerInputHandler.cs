using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfig;

    private CarController carController;

    private Weapon weapon;

    [SerializeField]
    private MeshRenderer playerMesh;

    private PlayerControls controls;

    private Gamepad playerGamepad;
    private float shootValue = 0.0f;


    private void Awake()
    {
        carController = GetComponent<CarController>();
        weapon = GetComponent<Weapon>();
        controls = new PlayerControls();
    }

    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        playerMesh.material = config.playerMaterial;
        config.Input.onActionTriggered += Input_onActionTriggered;

        weapon.SetPlayerConfiguration(config);

       
        playerConfig.PlayerUI = this.gameObject; 

        InputDevice device = config.Input.devices.FirstOrDefault(d => d is Gamepad);
        if (device != null)
        {
            playerGamepad = (Gamepad)device;
            Debug.Log("Gamepad connected for player: " + config.PlayerIndex);
        }

    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (obj.action.name == controls.Player.Movement.name)
        {
            OnMove(obj);
        }

        if (obj.action.name == controls.Player.Brakes.name)
        {
            OnBrakes(obj);
        }

        if (obj.action.name == controls.Player.Shoot.name)
        {
            OnShoot(obj);
        }

        if (obj.action.name == controls.Player.Mines.name)
        {
            OnMine(obj);
        }

        if (obj.action.name == controls.Player.Acceleration.name)
        {
            OnAcceleration(obj);
        }

    }

    public void OnMove(CallbackContext context)
    {
        if (carController != null)
        {
            carController.SetInputVector(context.ReadValue<Vector2>());

        }
    }

    public void OnAcceleration(CallbackContext context)
    {
        carController.SetInputAcceleration(context.ReadValue<float>() == 1 ? 1.0f : 0.0f);
    }

    public void OnBrakes(CallbackContext context)
    {
        if (carController != null)
        {
            carController.SetInputBrakes(context.ReadValue<float>() == 1 ? 1.0f : 0.0f);
        }
    }

    public void OnShoot(CallbackContext context)
    {
        shootValue = context.ReadValue<float>();

        if (weapon != null)
        {
            weapon.SetInputShoot(context.ReadValue<float>() == 1 ? 1.0f : 0.0f);
        }

        if (playerGamepad != null && shootValue == 1f)
        {
            StartVibration();
        }
        else
        {
           
            StopVibration();
        }

    }

    public void OnMine(CallbackContext context)
    {
        if (weapon != null)
        {
            weapon.SetInputMine(context.ReadValue<float>() == 1 ? 1.0f : 0.0f);
        }
    }

    private float vibrationHighFrequency = 0.8f;
    private float vibrationLowFrequency = 0.5f;

    public void StartVibration()
    {
        if (playerGamepad != null)
        {
            playerGamepad.SetMotorSpeeds(vibrationLowFrequency, vibrationHighFrequency);

        }
    }


    public void StopVibration()
    {
        if (playerGamepad != null)
        {
            playerGamepad.SetMotorSpeeds(0f, 0f);
        }
    }

}
