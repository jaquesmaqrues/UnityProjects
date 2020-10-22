using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This code was taken and modified from project 1
public class CameraController : MonoBehaviour
{
    public List<Camera> cameras;
    // Current camera 
    public int currentCameraIndex;
    // Use this for initialization
    void Start(){
        currentCameraIndex = 0;
        // Turn all cameras off, except the first default one
        for(int i = 1; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
        // If any cameras were added to the controller, enable the first one
        if(cameras.Count > 0){
            cameras[0].gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update(){
        // Press the 'C' key to cycle through cameras in the array
        if(Input.GetKeyDown(KeyCode.C)){
            // Cycle to the next camera
            currentCameraIndex++;
            // If cameraIndex is in bounds, set this camera active and last one inactive
            if(currentCameraIndex < cameras.Count)
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
            // If last camera, cycle back to first camera
            else
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
        }
    }
}
