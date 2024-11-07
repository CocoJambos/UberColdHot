using UnityEngine;
using UnityEngine.Events;

public class OnDevelopmentBuildEvents : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onDevelopmentBuild;
    [SerializeField]
    private UnityEvent onProductionBuild;

    private void Awake()
    {
#if DEVELOPMENT_BUILD
        onDevelopmentBuild.Invoke();
#else
        onProductionBuild.Invoke();
#endif
    }
}
