using UnityEngine;

public class CameraScript : MonoBehaviour
{

    float camera_speed = 0.03f;

    void Update(){

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 position = this.transform.position;
            position.y += camera_speed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 position = this.transform.position;
            position.y -= camera_speed;
            this.transform.position = position;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 position = this.transform.position;
            position.x -= camera_speed;
            this.transform.position = position;
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 position = this.transform.position;
            position.x += camera_speed;
            this.transform.position = position;
        } 

        if (Input.GetKey(KeyCode.C))
        {
            Vector3 position = this.transform.position;
            position.z += camera_speed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.X))
        {
            Vector3 position = this.transform.position;
            position.z -= camera_speed;
            this.transform.position = position;
        }

        if (Input.GetKey(KeyCode.A))
        {
            camera_speed += 0.01F;
        }
        if (Input.GetKey(KeyCode.S))
        {
            camera_speed -= 0.01F;
        }
    }
}
