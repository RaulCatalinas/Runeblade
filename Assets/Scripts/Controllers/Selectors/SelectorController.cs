using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectorController : MonoBehaviour
{
    [SerializeField] private Button[] menuButtons;
    [SerializeField] private RectTransform selectorArrow;

    void Start()
    {
        foreach (var button in menuButtons)
        {
            button.GetComponent<EventTrigger>();

            var trigger = button.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
            };

            entry.callback.AddListener(_ => MoveArrowTo(button));
            trigger.triggers.Add(entry);
        }
    }

    void MoveArrowTo(Button button)
    {
        var buttonRect = button.GetComponent<RectTransform>();

        selectorArrow.position = new Vector3(
            selectorArrow.position.x,
            buttonRect.position.y,
            selectorArrow.position.z
        );
    }
}
