using UnityEngine;

public class PhysicalButton : MonoBehaviour
{
    public bool IsPressed { get; private set; } = false;

    public void Press()
    {
        IsPressed = true;
    }
}
