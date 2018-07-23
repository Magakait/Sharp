using System.Collections;

using UnityEngine;

public class PatternNothing : MonoBehaviour
{
    public new SpriteRenderer renderer;

    [Space(10)]
    public int size;
    public int pixelsPerUnit;
    public float iterationRate;

    public bool Playing { get; set; } = true;

    private Texture2D texture;

    private int lifeMin;
    private int lifeMax;

    private int deathMin = 1;
    private int deathMax;

    private bool[,] gridNew;
    private bool[,] gridOld;

    private void Awake()
    {
        texture = new Texture2D(size, size, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Point
        };

        gridNew = new bool[size + 2, size + 2];
        gridOld = new bool[size + 2, size + 2];
    }

    public void Restart()
    {
        lifeMin = Random.Range(1, 8);
        lifeMax = Random.Range(lifeMin, 8);
        deathMax = Random.Range(deathMin, 8);

        for (int i = 1; i <= size; i++)
            for (int j = 1; j <= size; j++)
                gridNew[i, j] = false;
        gridNew[size / 2, size / 2] = true;

        StopAllCoroutines();
        StartCoroutine(Generate());
    }

    private void Display()
    {
        for (int i = 1; i <= size; i++)
            for (int j = 1; j <= size; j++)
                texture.SetPixel(i, j, gridNew[i, j] ? Color.white : Color.clear);

        texture.Apply();
        renderer.sprite = Sprite.Create
        (
            texture,
            new Rect(0, 0, size, size),
            Vector2.one * 0.5f,
            pixelsPerUnit
        );
    }

    private IEnumerator Generate()
    {
        for (int k = 0, n = (size - 10) / 2; k < n; k++)
        {
            for (int i = 1; i <= size; i++)
                for (int j = 1; j <= size; j++)
                    gridOld[i, j] = gridNew[i, j];

            for (int i = 1; i <= size; i++)
                for (int j = 1; j <= size; j++)
                {
                    int moore = GetMoore(i, j);
                    if (gridOld[i, j])
                        gridNew[i, j] = lifeMin <= moore && moore <= lifeMax;
                    else
                        gridNew[i, j] = deathMin <= moore && moore <= deathMax;
                }

            Display();

            yield return new WaitForSeconds(iterationRate);
            yield return new WaitUntil(() => Playing);
        }
    }

    private int GetMoore(int x, int y)
    {
        int result = 0;
        for (int i = y - 1; i <= y + 1; i++)
            for (int j = x - 1; j <= x + 1; j++)
                if (gridOld[i, j])
                    result++;

        return gridOld[x, y] ? result - 1 : result;
    }
}