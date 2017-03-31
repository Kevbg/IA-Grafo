using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour {
    public GameObject scripts;
    public Text matrixText;

    public void DisplayMatrix() {
        matrixText.text = string.Empty;
        int[][] matrix = scripts.GetComponent<EdgeBuilder>().matrix;

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

        // Deixa o painel na frente dos outros elementos no canvas
        transform.parent.SetAsLastSibling();
    }
}
