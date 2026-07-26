using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    private static PlayerMarker _instance;
    public static PlayerMarker Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

    }
}
