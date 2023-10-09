using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private Texture2D cursorTexture;
        
        private CursorMode cursorMode = CursorMode.Auto;
        private Vector2 hotSpot = Vector2.zero;


        public void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }
}
