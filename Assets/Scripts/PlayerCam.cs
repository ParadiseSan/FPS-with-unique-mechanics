using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{

    [SerializeField] float sensitivity;

    float xRotation, yRotation;

    [SerializeField] Transform orientation;
    [SerializeField] Transform camHolder;
 

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.startGame)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void DoFOV(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
