using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pathfinder : MonoBehaviour {
    public Color startPointColor;
    public Color endPointColor;
    public Color edgeColor;
    public float edgeWidth;
    public Toggle[] buttons;
    public string pathTaken { get; private set; }
    private Color startPointOriginalColor;
    private GameObject startPoint;
    private GameObject endPoint;
    private List<Edge> edges;

    void Update() {
        // Verifica o input do ponto inicial (startPoint)
        if (Input.GetMouseButtonDown(0) && startPoint == null) {
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "Vertex") {
                SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
                startPointOriginalColor = sr.color;
                sr.color = startPointColor;
                startPoint = hit.collider.gameObject;
                print("Start point set: " + startPoint.name);
            }
        }
        // Verifica o input do ponto final (endPoint)
        else if (Input.GetMouseButtonDown(0) && startPoint != null) {
            RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "Vertex" && hit.transform.gameObject != startPoint) {
                SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
                Color originalColor = sr.color;
                sr.color = endPointColor;
                endPoint = hit.collider.gameObject;
                print("End point set: " + endPoint.name + ", searching for the shortest path");
                FindShortestPath();
            }
        }
        // Cancela a criação do ponto inicial
        else if (Input.GetMouseButtonDown(1) && startPoint != null) {
            startPoint.GetComponent<SpriteRenderer>().color = startPointOriginalColor;
            startPoint = null;
        }
    }

    // Converte a posição do mouse na tela e descarta o eixo Z
    private Vector2 GetMousePos() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(mousePos.x, mousePos.y);
    }

    private void FindShortestPath() {
        // Executa uma busca do ponto incial até o ponto final
        List<GameObject> path = BFS(startPoint, endPoint);
        HighlightEdges(path);

        // Formatação do caminho percorrido
        string[] pathArr = new string[path.Count];
        for (int i = 0; i < path.Count; i++) {
            pathArr[i] = path[i].name;
        }

        Array.Reverse(pathArr);
        pathTaken = string.Join(" -> ", pathArr);
        print(pathTaken);

        GetComponent<ModeSwitcher>().SwitchMode();
        ToggleUIButtons();
    }

    // Algoritmo de Busca em Largura para encontrar o menor caminho
    private List<GameObject> BFS(GameObject root, GameObject goal) {
        List<GameObject> visited = new List<GameObject>();
        Queue<GameObject> q = new Queue<GameObject>();

        // Começa a busca a partir da raiz (root)
        root.GetComponent<Vertex>().parent = null;
        q.Enqueue(root);
        visited.Add(root);

        // Executa a busca até que não haja mais vértices na fila
        while (q.Count > 0) {
            GameObject current = q.Dequeue();

            // Ao encontrar o objetivo, percorre seus pais até chegar na raiz (pai == null)
            if (current == goal) {
                GameObject parent = current.GetComponent<Vertex>().parent;
                List<GameObject> path = new List<GameObject>();
                path.Add(current);
                print("Path found!");

                while (parent != null) {
                    path.Add(parent);
                    parent = parent.GetComponent<Vertex>().parent;
                }

                // Retorna o caminho encontrado após chegar na raiz
                return path;
            }

            // Percorre todos os vértices adjacentes e os insere na fila
            foreach (GameObject adjacent in current.GetComponent<Vertex>().connections) {
                if (!visited.Contains(adjacent)) {
                    visited.Add(adjacent);
                    adjacent.GetComponent<Vertex>().parent = current;
                    q.Enqueue(adjacent);
                }
            }
        }

        return null;
    }

    // Realce das arestas para indicar o caminho
    private void HighlightEdges(List<GameObject> path) {
        edges = new List<Edge>();

        foreach (GameObject vertex in path) {
            if (vertex.transform.childCount > 0) {
                Edge[] children = vertex.GetComponentsInChildren<Edge>();

                foreach (Edge edge in children) {
                    if (path.Contains(edge.parentVertex) && path.Contains(edge.childVertex)) {
                        LineRenderer lr = edge.GetComponent<LineRenderer>();
                        lr.startWidth = edgeWidth;
                        lr.endWidth = edgeWidth;
                        lr.startColor = edgeColor;
                        lr.endColor = edgeColor;

                        edges.Add(edge);
                    }
                }
            }
        }
    }

    // Habilita ou habilita certos botões da interface
    private void ToggleUIButtons() {
        foreach(Toggle t in buttons) {
            switch (t.interactable) {
                case true:
                    t.interactable = false;
                    break;
                case false:
                    t.interactable = true;
                    break;
            }
        }
    }

    // Distância: pesoAresta1 + pesoAresta2 + pesoArestaN
    public float Distance() {
        float distance = 0;

        foreach (Edge edge in edges) {
            distance += edge.weight;
        }

        return distance;
    }

    public float ManhattanDistance() {
        return Mathf.Abs(startPoint.transform.position.x - endPoint.transform.position.x) + 
               Mathf.Abs(startPoint.transform.position.y - endPoint.transform.position.y);
    }
}
