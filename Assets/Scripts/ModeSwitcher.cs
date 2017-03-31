using UnityEngine;
using UnityEngine.UI;

public class ModeSwitcher : MonoBehaviour {
    public Image button;
    public Color pressedColor;
    private Color originalColor;

    void Start() {
        originalColor = button.color;
    }

    // Alterna entre os modos de construção de aresta e de busca de caminho
	public void SwitchMode() {
        EdgeBuilder edgeBuilder = GetComponent<EdgeBuilder>();
        Pathfinder pathfinder = GetComponent<Pathfinder>();

        switch (edgeBuilder.enabled) {
            case true:
                edgeBuilder.enabled = false;
                pathfinder.enabled = true;
                button.color = pressedColor;
                break;
            case false:
                edgeBuilder.enabled = true;
                pathfinder.enabled = false;
                button.color = originalColor;
                break;
        }
    }
}
