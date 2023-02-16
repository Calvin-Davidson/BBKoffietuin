using UnityEngine;

public class RouteMenuItem : MonoBehaviour
{
    private Route.Route route;
    private Clickable _clickable;
    private void Awake()
    {
        _clickable = GetComponent<Clickable>();
        if (_clickable == null)
        {
            Debug.LogWarning("No clickable component found on " + gameObject.name);
            Destroy(this);
        }
        
        _clickable.onButtonUp.AddListener(TryStartRoute);
    }

    /// <summary>
    /// Checks if everything is ready to start the route if not handle it.
    /// </summary>
    private void TryStartRoute()
    {
        //try starting the root 
        GpsService.Instance.RequestPermission(
            () =>
            {
                StartRoute();
            },
            () =>
            {
                StartRoute();
            }, 
            ()=>{}, 
            ()=>{});
    }

    private void StartRoute()
    {
        
    }
}
