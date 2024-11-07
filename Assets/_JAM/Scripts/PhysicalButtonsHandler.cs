using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalButtonsHandler : MonoBehaviour
{
    [SerializeField]
    private List<PhysicalButton> buttons;
    [SerializeField]
    private UnityEvent onAllButtonsPressed;

    private bool allButtonsPressed = false;

    private void Update()
    {
        if(!allButtonsPressed && buttons.All(button => button.IsPressed))
        {
            onAllButtonsPressed.Invoke();
            allButtonsPressed = true;
        }
    }
}
