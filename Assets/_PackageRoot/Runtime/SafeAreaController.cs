using UnityEngine;

public class SafeAreaController : MonoBehaviour
{
    protected RectTransform safeAreaRect;
	protected Canvas canvas;

    private Rect lastSafeArea = Rect.zero;
    private bool screenChangeVarsInitialized = false;
    private ScreenOrientation lastOrientation = ScreenOrientation.Portrait;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        safeAreaRect = GetComponent<RectTransform>();

        if (!screenChangeVarsInitialized)
        {
            lastOrientation = Screen.orientation;
            lastSafeArea = Screen.safeArea;

            screenChangeVarsInitialized = true;
        }
    }

    private void Start()
    {
        ApplySafeArea();
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            if (Screen.orientation != lastOrientation)
            {
                OrientationChanged();
                SafeAreaChanged();
            }

            if (Screen.safeArea != lastSafeArea)
                SafeAreaChanged();
        }
    }

    protected virtual void ApplySafeArea()
    {
        if (safeAreaRect == null)
            return;

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        safeAreaRect.anchorMin = anchorMin;
        safeAreaRect.anchorMax = anchorMax;
    }

    private void OrientationChanged()
    {
        lastOrientation = Screen.orientation;
    }

    private void SafeAreaChanged()
    {
        if (lastSafeArea == Screen.safeArea)
            return;

        if (Screen.safeArea.width == float.NaN || Screen.safeArea.height == float.NaN)
            return;

        if (Screen.safeArea.width == 0 || Screen.safeArea.height == 0)
            return;

        lastSafeArea = Screen.safeArea;

        ApplySafeArea();
    }
}
