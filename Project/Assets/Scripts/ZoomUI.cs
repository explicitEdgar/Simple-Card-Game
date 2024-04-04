using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{   
    public float zoomSize = 1.2f;

    public Texture2D texture;

    void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.localScale = new Vector3(zoomSize,zoomSize,1.0f);
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.localScale = Vector3.one;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
