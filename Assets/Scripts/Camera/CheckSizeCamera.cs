using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSizeCamera : MonoBehaviour
{
    public SpriteRenderer background;

    private void Awake()
    {
        Camera.main.orthographicSize = background.bounds.size.x * Screen.height / Screen.width * 0.5f;
    }

}
