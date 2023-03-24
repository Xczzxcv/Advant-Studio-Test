using UnityEngine;

namespace Ui
{
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    private Rect _lastSafeArea;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateSafeArea();
    }

    private void Update()
    {
        UpdateSafeArea();
    }

    private void UpdateSafeArea()
    {
        if (_lastSafeArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        var safeAreaRect = Screen.safeArea;

        var anchorMin = safeAreaRect.position;
        var anchorMax = anchorMin + safeAreaRect.size;
        
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;

        _lastSafeArea = Screen.safeArea;
    }
}
}