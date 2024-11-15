using UnityEngine;

[ExecuteAlways]
public class LightningManager : MonoBehaviour
{
    // Fuente: https://www.youtube.com/watch?v=m9hj9PdO328
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightningPreset Preset;
    //Variables
    /// <summary>
    /// Hora actual
    /// </summary>
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    /// <summary>
    /// Hora en la que se hace de día
    /// </summary>
    [SerializeField, Range(0.1f, 23.9f)] private float dayTime;
    /// <summary>
    /// Hora en la que se hace de noche
    /// </summary>
    [SerializeField, Range(0.1f, 23.9f)] private float nightTime;

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            TimeOfDay = TimeManager.Hour + (TimeManager.Minute / 0.6f)/100;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }
    /// <summary>
    /// Cambia los colores y orientación de la luz dependiendo de timePercent
    /// </summary>
    /// <param name="timePercent">Porcentaje de la hora actual, 0..1</param>
    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            //Debug.Log("Hour: " + TimeOfDay + ", TimePerceent: " + timePercent);
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            float lightRotation = CalcLight();
            DirectionalLight.transform.localRotation = Quaternion.Slerp(DirectionalLight.transform.localRotation, Quaternion.Euler(lightRotation, 170f, 0f), Time.deltaTime * 10f);
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
    /// <summary>
    /// Calcula en grados la rotación en X que debe tener la luz en un momento dado (TimeOfDay)
    /// </summary>
    /// <returns>La rotación en X</returns>
    private float CalcLight()
    {
        if (TimeOfDay == nightTime)
            return 180f;
        if (TimeOfDay == dayTime)
            return 0f;
        if (TimeOfDay < nightTime && TimeOfDay > dayTime) 
            return ValueBetweenRang(TimeOfDay, dayTime, nightTime, 0f, 180f); // Day
        if (TimeOfDay > nightTime) 
            return ValueBetweenRang(TimeOfDay - 24, nightTime - 24, dayTime, 180f, 360f); // Night before midnight
        return ValueBetweenRang(TimeOfDay, nightTime - 24, dayTime, 180f, 360f); // Night after midnight
    }
    /// <summary>
    /// Dados dos valores que forman un rango, encontrar el equivalente de un valor dentro de otro rango, creo
    /// </summary>
    /// <param name="value">Valor a convertir</param>
    /// <param name="initialValue">Valor inicial del primer rango</param>
    /// <param name="finalValue">Valor final del segundo rango</param>
    /// <param name="initialValueConversion">Valor inicial del segundo rango</param>
    /// <returns>Valor equivalente en el segundo rango</returns>
    private float ValueBetweenRang(float value, float initialValue, float finalValue, float initialValueConversion, float finalValueConversion)
    {
        float normalizedValue = (value - initialValue) / (finalValue - initialValue);
        return normalizedValue * (finalValueConversion - initialValueConversion) + initialValueConversion;
    }

    public float GetDayTime()
    {
        return dayTime;
    }
    public float GetNightTime()
    {
        return nightTime;
    }
}
