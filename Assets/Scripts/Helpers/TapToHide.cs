using UnityEngine;
using UnityEngine.EventSystems;

public class TapToHide : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _objectToHide;
    public void OnPointerDown(PointerEventData eventData)
    {
        _objectToHide.SetActive(false);
    }
}
