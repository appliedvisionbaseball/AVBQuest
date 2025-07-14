using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonXX : MonoBehaviour, IPointerClickHandler
{
	public UnityEvent invoke;

	// Use this for initialization


	void OnMouseUpAsButton ()
	{
		invoke.Invoke ();
	}


    public void OnPointerClick(PointerEventData eventData)
    {
		invoke.Invoke();
	}
}
