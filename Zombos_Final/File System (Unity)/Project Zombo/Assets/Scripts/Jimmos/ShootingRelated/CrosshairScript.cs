using UnityEngine;

[ExecuteInEditMode]
public class CrosshairScript : MonoBehaviour
{
    public float height = 10f;
    public float width = 2f;
    public float defaultSpread = 10f;
    public Color color = Color.grey;
    public float fadeSpeed = 3f;
    public bool resizeable = false;
    public float resizedSpread = 20f;
    public float resizeSpeed = 3f;

    public float spread { get; private set; }
    private bool resizing = false;
    private InputManager input;

    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
        // Set spread
        spread = defaultSpread;
    }

    void Update()
    {
        if (input.MouseFireHold || input.MouseFireDown) { resizing = true; } else { resizing = false; }

        if (input.MouseAimHold)
        {
            color.a = Mathf.Lerp(color.a, 0, fadeSpeed * Time.deltaTime);
        }
        else
        {
            color.a = Mathf.Lerp(color.a, 1, fadeSpeed * Time.deltaTime);
        }

        if (resizeable)
        {
            if (resizing)
            {
                // Increase spread 
                spread = Mathf.Lerp(spread, resizedSpread, resizeSpeed * Time.deltaTime);
            }
            else
            {
                // Decrease spread
                spread = Mathf.Lerp(spread, defaultSpread, resizeSpeed * Time.deltaTime);
            }

            // Clamp spread
            spread = Mathf.Clamp(spread, defaultSpread, resizedSpread);
        }
    }

    void OnGUI()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        //up rect
        GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - height / 2) + spread / 2, width, height), texture);

        //down rect
        GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - height / 2) - spread / 2, width, height), texture);

        //left rect
        GUI.DrawTexture(new Rect((Screen.width / 2 - height / 2) + spread / 2, Screen.height / 2 - width / 2, height, width), texture);

        //right rect
        GUI.DrawTexture(new Rect((Screen.width / 2 - height / 2) - spread / 2, Screen.height / 2 - width / 2, height, width), texture);
    }
}