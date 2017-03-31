using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour {
    public GameObject scripts;
    public Text matrixText;
    public Text pathText;
    public Text distanceText;
    public Text manhattanDistanceText;
    private Pathfinder pathfinder {
        get { return scripts.GetComponent<Pathfinder>(); }
    }

    public void DisplayMatrix() {
        matrixText.text = string.Empty;
        float[][] matrix = scripts.GetComponent<EdgeBuilder>().matrix;

        // Disposição da matriz no painel
        for (int i = 0; i < matrix.Length; i++) {
            string[] arr = new string[matrix[i].Length];

            for (int j = 0; j < matrix[i].Length; j++) {
                arr[j] = matrix[i][j].ToString();
            }

            matrixText.text += string.Join("|", arr) + "\n";
        }

        // Ajustes no tamanho do painel e da caixa de texto para acomodar a matriz
        GetComponent<RectTransform>().sizeDelta = new Vector2(matrix.Length * 15 + 30, matrix.Length * 20 + 10);
        matrixText.GetComponent<RectTransform>().sizeDelta = new Vector2(matrix.Length * 15 + 1, matrix.Length * 20);

        BringToFront();
    }

    // Deixa o painel na frente dos outros elementos no canvas
    private void BringToFront() {
        transform.SetAsLastSibling();
        transform.parent.SetAsLastSibling();
    }

    public void DisplayPathTaken() {
        pathText.text = pathfinder.pathTaken;
        BringToFront();
    }

    public void DisplayDistance() {
        distanceText.text = "Distância Percorrida: " + pathfinder.Distance();
        BringToFront();
    }

    public void DisplayManhattanDistance() {
        manhattanDistanceText.text = "Distância Manhattan: " + pathfinder.ManhattanDistance();
        BringToFront();
    }
}
