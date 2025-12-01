using UnityEngine;
using UnityEngine.InputSystem;

namespace jetfighter.movement
{
[RequireComponent(typeof(PlayerInput))]
public class playermovement : mover
{
    private void OnMove(InputValue value)
    {
        Vector3 PlayerInput = new Vector3(
            value.Get<Vector2>().x,
            value.Get<Vector2>().y  ,0
             );
        currentinput = PlayerInput;
}
}
}