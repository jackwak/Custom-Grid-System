using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClick : MonoBehaviour
{
    PlayerInput playerInput;
    Vector3 mousePosition;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.Player.OnLeftMouseClicked.started += context =>
        {
            mousePosition = Camera.main.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue());

            if (Physics.Raycast(mousePosition , Vector3.forward, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Grid grid))
                {
                    grid.DisplayX(true);
                    GridManager.Instance.CheckForMatches(grid);
                }
            }
        };
    }

   

    private void OnEnable()
    {
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
