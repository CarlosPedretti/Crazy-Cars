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

        // Asignar el PlayerUI a la configuración del jugador
        playerConfig.PlayerUI = this.gameObject; // O utiliza el objeto de la UI del jugador si está en otro GameObject
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
        if (weapon != null)
        {
            weapon.SetInputShoot(context.ReadValue<float>() == 1 ? 1.0f : 0.0f);
        }
    }

    public void OnMine(CallbackContext context)
    {
        if (weapon != null)
        {
            weapon.SetInputMine(context.ReadValue<float>() == 1 ? 1.0f : 0.0f);
        }
    }


}
