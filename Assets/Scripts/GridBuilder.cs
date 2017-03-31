using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridBuilder : MonoBehaviour {
    [Range(2, 6)]
    public int gridSize = 2;
    public GameObject vertex;
    public GameObject vertexLabel;
    public GameObject canvas;
    public GameObject slider;
    public float vertexSpacing = 2;
    private List<GameObject> vertices;
    private List<GameObject> vertexLabels;

    // Instancia uma nova grade, removendo a atual caso exista
    public void Initialize() {
        if (vertices != null) {
            ClearGrid();
        }

        vertices = new List<GameObject>();
        vertexLabels = new List<GameObject>();
        CheckSliderValue();
        CreateNewGrid();
    }

    public void CreateNewGrid() {
        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                // Centraliza a grade conforme novos vértices são criados (da parte superior esquerda à parte inferior direita)
                Vector2 newVertexPos = new Vector2(-(gridSize - 1) * vertexSpacing / 2 + (j * vertexSpacing),
                                                    (gridSize - 1) * vertexSpacing / 2 - (i * vertexSpacing));
                GameObject newVertex = Instantiate(vertex, newVertexPos, Quaternion.identity);
                vertices.Add(newVertex);
                newVertex.name = (j + (i * gridSize)).ToString();

                // Label para o novo vértice que foi criado
                Vector3 newLabelPos = Camera.main.WorldToViewportPoint(new Vector3(newVertexPos.x, newVertexPos.y, 0));
                GameObject newLabel = Instantiate(vertexLabel, Camera.main.WorldToViewportPoint(newVertexPos), Quaternion.identity, canvas.transform);
                vertexLabels.Add(newLabel);
                newLabel.name = "Vertex Label " + newVertex.name;
                newLabel.GetComponent<Text>().text = newVertex.name;

                // Ancora as labels em sua nova posição
                newLabel.GetComponent<RectTransform>().anchorMin = newLabelPos;
                newLabel.GetComponent<RectTransform>().anchorMax = newLabelPos;
                newLabel.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            }
        }
    }

    // Remove a grade atual
    public void ClearGrid() {
        for (int i = 0; i < vertices.Count; i++) {
            Destroy(vertices[i]);
            Destroy(vertexLabels[i]);
        }

        vertices.Clear();
        vertexLabels.Clear();
    }

    // Verifica o valor do slider para poder sincronizar com a variável gridSize
    public void CheckSliderValue() {
        Slider s = slider.GetComponent<Slider>();

        s.onValueChanged.AddListener(SetSliderValue);
        SetSliderValue(s.value);
    }

    public void SetSliderValue(float value) {
        gridSize = (int)value;
    }
}
