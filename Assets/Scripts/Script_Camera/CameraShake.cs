using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{ 
    private Camera c;
    [SerializeField] private float cameraShakeDuration = 0.5f;
    [SerializeField] private float cameraShakeDecreaseFactor = 1f;
    [SerializeField] private float cameraShakeIntensity = 1f;

    public void Awake()
    {
        c = GetComponent<Camera>();
    }

    private void ShakeCamera()
    {
        StartCoroutine(ShakeRoutine(cameraShakeIntensity, cameraShakeDecreaseFactor, cameraShakeDuration));
    }
    
    public void ShakeCamera(float i, float d, float t)
    {
        StartCoroutine(ShakeRoutine(i, d, t));
    }

    public IEnumerator ShakeRoutine(float i, float d, float t)
    {
        while (t >= 0f)
        {
            Vector2 dir = Random.insideUnitCircle * i;
            c.transform.localPosition = new Vector3(c.transform.localPosition.x - dir.x, c.transform.localPosition.y -dir.y, c.transform.localPosition.z);
            t -= Time.deltaTime * d;
            yield return null;
        }
    }

    public void OnEnable()
    {
        Player_DataHandler.ShootGun.AddListener(ShakeCamera);
    }

    public void OnDisable()
    {
       Player_DataHandler.ShootGun.RemoveListener(ShakeCamera);
    }
}