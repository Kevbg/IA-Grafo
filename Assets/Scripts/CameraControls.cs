using UnityEngine;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour {
    public GameObject gridSizeSlider;
    public GameObject target;
	
	void Start () {
        if (target != null) {
            Vector3 targetPosX = new Vector3(target.transform.position.x, 
                                             transform.position.y, 
                                             transform.position.z);
            transform.position = targetPosX;
        }
    }

    public void ScaleWithGrid(int offset) {
        int gridSize = (int)gridSizeSlider.GetComponent<Slider>().value;
        GetComponent<Camera>().orthographicSize = gridSize + offset;
    }
}
