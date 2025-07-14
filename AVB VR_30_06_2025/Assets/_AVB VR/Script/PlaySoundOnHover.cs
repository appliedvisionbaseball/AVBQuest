using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnHover : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioHandler.instance.ButtonHover_AudioPlay();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioHandler.instance.ButtonClick_AudioPlay();
    }
}
