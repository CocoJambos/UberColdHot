using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnPressed;
    public bool IsPressed { get; private set; } = false;

    public void Press()
    {
        if (IsPressed) {return;}
        IsPressed = true;
        Debug.Log(gameObject.name + " is pressed");
        
        if(OnPressed != null)
            OnPressed.Invoke();
    }
}
