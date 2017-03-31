using System;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBuilder : MonoBehaviour {
    public GameObject edge;
    private GameObject currentEdge;
    private LineRenderer currentLine;
    private List<GameObject> edges;
    private int gridSize;
    private GameObject firstVertex;
    private GameObject secondVertex;
    public float[][] matrix { get; private set; }

    // Instancia uma nova matriz e lista de arestas
    public void Initialize() {
        if (edges != null) {
            ClearGrid();
        }

        gridSize = GetComponent<GridBuilder>().gridSize;
        edges = new List<GameObject>();

        CreateNewMatrix();
    }

    // Cria uma nova matriz
    public void CreateNewMatrix() {
        matrix = new float[(int)Mathf.Pow(gridSize, 2)][];

        for (int i = matrix.Length; i > 0; i--) {
            matrix[matrix.Length - i] = new float[i];
        }
    }

    // Remove a matriz e as arestas atuais
    public void ClearGrid() {
        for (int i = 0; i < edges.Count; i++) {
            Destroy(edges[i]);
        }
        edges.Clear();
        Array.Clear(matrix, 0, matrix.Length);
        CreateNewMatrix();
    }

    void Update() {
        // Verifica onde será conectada a aresta
        if (currentEdge != null) {
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);
            DrawLine(currentLine, GetMousePos());

            if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.tag == "Vertex"
                && hit.collider != currentEdge.GetComponentInParent<Collider2D>()) {
                int row, column;
                // Ordena a posição das arestas na matriz
                if (int.Parse(hit.collider.name) > int.Parse(currentEdge.transform.root.name)) {
                    row = int.Parse(currentEdge.transform.root.name);
                    column = int.Parse(hit.collider.name) - row;
                } else {
                    row = int.Parse(hit.collider.name);
                    column = int.Parse(currentEdge.transform.root.name) - row;
                }

                // Verifica se a aresta já foi adicionada à matriz de adjacências
                if (matrix[row][column] == 0) {
                    // Conecta a aresta a um outro vértice
                    secondVertex = hit.transform.gameObject;
                    firstVertex.GetComponent<Vertex>().connections.Add(secondVertex);
                    secondVertex.GetComponent<Vertex>().connections.Add(firstVertex);
                    DrawLine(currentLine, hit.collider.transform.position);
                    currentEdge.GetComponent<Edge>().childVertex = hit.transform.gameObject;
                    currentEdge.name = "Edge(" + currentEdge.transform.root.name + ", " + hit.transform.name + ")";

                    float angle = Vector3.Angle(currentLine.GetPosition(1) - currentEdge.transform.root.position, Vector3.right);
                    // Verificar se a aresta é diagonal ou não
                    if (angle % 90 == 0) {
                        matrix[row][column] = 1;
                        currentEdge.GetComponent<Edge>().weight = 1;
                    } else {
                        //matrix[row][column] = Mathf.Sqrt(2);
                        matrix[row][column] = 1;
                        currentEdge.GetComponent<Edge>().weight = Mathf.Sqrt(2);
                    }

                    //matrix[row][column] = 1;

                    currentEdge = null;
                    currentLine = null;
                    firstVertex = null;
                    secondVertex = null;
                } else {
                    Destroy(currentEdge);
                    currentLine = null;
                    firstVertex = null;
                    secondVertex = null;
                }

                // Cancela a criação da nova aresta
            } else if (Input.GetMouseButtonDown(1)) {
                Destroy(currentEdge);
                currentLine = null;
                firstVertex = null;
                secondVertex = null;
            }

          // Instancia uma nova aresta
        } else if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "Vertex") {
                firstVertex = hit.transform.gameObject;
                GameObject newEdge = Instantiate(edge, hit.transform.position, Quaternion.identity, hit.transform);
                edges.Add(newEdge);
                currentLine = newEdge.GetComponent<LineRenderer>();
                currentEdge = newEdge;
                currentEdge.GetComponent<Edge>().parentVertex = hit.transform.gameObject;
            }
        }
    }

    // Converte a posição do mouse na tela e descarta o eixo Z
    private Vector2 GetMousePos() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(mousePos.x, mousePos.y);
    }

    // Desenha a aresta
    private void DrawLine(LineRenderer line, Vector2 endPoint) {
        line.SetPosition(0, line.transform.position);
        line.SetPosition(1, endPoint);

        if (!line.enabled) {
            line.enabled = true;
        }
    }
}
