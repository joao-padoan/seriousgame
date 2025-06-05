using UnityEngine;

public class FadeCamera : MonoBehaviour
{
    public bool fadeTrigger;
    public float speedScale = 1f;
    public Color fadeColor = Color.black;
    public AnimationCurve Curve = new AnimationCurve(
        new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f),
        new Keyframe(1, 0));
    public bool startFadedOut = false;

    private float alpha = 0f;
    private Texture2D texture;
    private int direction = 0; // -1 for fade in, 1 for fade out, 0 for idle
    private float time = 0f;
    public static FadeCamera instance;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Initialize alpha and texture
        alpha = startFadedOut ? 1f : 0f;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }

    private void Update()
    {
        // Check for fade trigger
        if (fadeTrigger && direction == 0) // Only trigger if idle
        {
            fadeTrigger = false; // Reset trigger

            if (alpha >= 1f) // Fully faded out, start fading in
            {
                direction = -1;
                time = 1f;
            }
            else if (alpha <= 0f) // Fully faded in, start fading out
            {
                direction = 1;
                time = 0f;
            }
        }
    }

    public void OnGUI()
    {
        // Draw the fade texture
        if (alpha > 0f)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }

        // Update alpha if fading
        if (direction != 0)
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = Mathf.Clamp01(Curve.Evaluate(time));

            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();

            // Stop fading if fully faded in or out
            if (alpha <= 0f || alpha >= 1f)
            {
                direction = 0;
            }
        }
    }
}
