using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float movementControl = 1f;
    [SerializeField] float xRange = 3f;
    [SerializeField] float yRange = 3f;
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionRollFactor = -2f;
    [SerializeField] float controllerPitchFactor = -10f;
    [SerializeField] float controllerRollFactor = -10f;


    float yThrow = 0f;
    float xThrow = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();

    }


    void ProcessRotation()
    {

        float pitch = transform.localPosition.y * positionPitchFactor + yThrow * controllerPitchFactor;

        float pitchDueToPlayerController = yThrow * controllerPitchFactor;


        float yaw = 0f;
        float roll = transform.localPosition.x * positionRollFactor + xThrow * controllerRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffSet = (xThrow * movementControl * Time.deltaTime);
        float yOffSet = (yThrow * movementControl * Time.deltaTime);

        float new_x_position = transform.localPosition.x + xOffSet;
        new_x_position = Mathf.Clamp(new_x_position, -xRange, xRange);

        float new_y_position = transform.localPosition.y + yOffSet;
        new_y_position = Mathf.Clamp(new_y_position, -yRange, yRange);




        transform.localPosition = new Vector3(new_x_position, new_y_position, transform.localPosition.z);
    }


}
