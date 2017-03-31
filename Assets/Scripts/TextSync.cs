using UnityEngine;
using UnityEngine.UI;

public class TextSync : MonoBehaviour {

    public bool syncOnStart;
    public GameObject target;
    public ComponentType type;
    public enum ComponentType {
        text,
        slider
    }

    void Start() {
        if (syncOnStart) {
            SyncText();
        }
    }

    // Sincroniza o texto deste GameObject com o target, de acordo com o tipo
    public void SyncText() {
        Text t = GetComponent<Text>();

        switch (type) {
            case ComponentType.text:
                t.text = target.GetComponent<Text>().text;
                break;
            case ComponentType.slider:
                t.text = target.GetComponent<Slider>().value.ToString();
                break;
        }
    }
}
