using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [Range(0, 24)]
    public float timeOfDay;

    public Light sun;
    public Light moon;
    public Volume skyVolume;
    public AnimationCurve starsCurve;

    private bool isNight;
    private PhysicallyBasedSky sky;

    public DataReaders dataReaders;

    void Start()
    {
        dataReaders = GetComponent<DataReaders>();
        skyVolume.profile.TryGet(out sky);
    }

    void Update()
    {
        if (dataReaders.dataQueue.TryDequeue(out float result))
        {
            timeOfDay = result;
        }
        UpdateTime();
    }

    private void UpdateTime()
    {
        // Update rotations
        UpdateRotations();

        // Update sky color
        UpdateSkyColor();

        // Switch between day and night
        DayNight();
    }

    private void UpdateRotations()
    {
        float alpha = timeOfDay / 24.0f;
        float sunRotation = Mathf.Lerp(-90, 270, alpha);
        float moonRotation = sunRotation - 180;

        sun.transform.rotation = Quaternion.Euler(sunRotation, -150.0f, 0);
        moon.transform.rotation = Quaternion.Euler(moonRotation, -150.0f, 0);
    }

    private void UpdateSkyColor()
    {
        float alpha = timeOfDay / 24.0f;
        sky.spaceEmissionMultiplier.value = starsCurve.Evaluate(alpha) * 5.0f;
    }

    private void DayNight()
    {
        if (isNight)
        {
            if (moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
            }
        }
        else
        {
            if (sun.transform.rotation.eulerAngles.x > 180)
            {
                StartNight();
            }
        }
    }

    private void StartDay()
    {
        isNight = false;
        SetShadows(sun, moon);
    }

    private void StartNight()
    {
        isNight = true;
        SetShadows(moon, sun);
    }

    private void SetShadows(Light mainLight, Light secondaryLight)
    {
        mainLight.shadows = LightShadows.Soft;
        secondaryLight.shadows = LightShadows.None;
    }
}
