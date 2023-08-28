using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Header("Elements")]
    [SerializeField] private GameObject bgTilePrefab;
    public Gem[] gems;
    public Gem[,] allGems;

    int gemToUse;
    private Gem gem;
    private Vector2 pos;
    private GameObject bgTile;
    void Start()
    {
        allGems = new Gem[width, height];

        SetupTheBoard();
    }
    private void SetupTheBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                pos = new Vector2(x, y);
                bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity, transform);
                bgTile.name = "BG Tile - " + x + ", " + y;

                gemToUse = Random.Range(0, gems.Length);
                SpawnGem(new Vector2Int(x,y), gems[gemToUse]);
            }
        }
    }
    private void SpawnGem(Vector2Int spawnPos , Gem gemToSpawn)
    {
        gem = Instantiate(gemToSpawn, new Vector3(spawnPos.x, spawnPos.y,0), Quaternion.identity);
        gem.transform.parent = transform;
        gem.name = "Gem - " + pos.x + ", " + pos.y;
        allGems[spawnPos.x, spawnPos.y] = gem;

        gem.SetupGem(spawnPos, this);
    }
}
