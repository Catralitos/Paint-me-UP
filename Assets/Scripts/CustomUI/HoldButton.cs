using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomUI
{
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
        [HideInInspector] public bool buttonPressed;
 
        public void OnPointerDown(PointerEventData eventData){
            buttonPressed = true;
        }
 
        public void OnPointerUp(PointerEventData eventData){
            buttonPressed = false;
        }
    }
}