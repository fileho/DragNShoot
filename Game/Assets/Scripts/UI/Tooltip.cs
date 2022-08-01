using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private Transform tooltipTransform;
    private CanvasGroup canvasGroup;
    private Text text;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    public static Tooltip instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        canvasGroup = tooltipTransform.GetComponent<CanvasGroup>();
        text = tooltipTransform.GetComponentInChildren<Text>();
    }

    private RectTransform RaycastButton()
    {
        var pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            var button = result.gameObject.GetComponent<Button>();
            if (button != null)
                return button.GetComponent<RectTransform>();
        }

        return null;
    }


    public void Show(string message)
    {
        var rect = RaycastButton();

        var p = rect.position;
        p.x += rect.rect.width / 2 + 10;

        tooltipTransform.position = p;
        text.text = message;
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
