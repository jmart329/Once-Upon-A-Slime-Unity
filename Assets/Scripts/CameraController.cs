using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool rotate;
    

    // Start is called before the first frame update
    void Start()
    {   
        rotate = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        rotate = Input.GetMouseButton(0);

    }


    private void LateUpdate()
    {
        if (rotate)
        {
            
        }
    }
}
