using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    // Fields
    private bool canGenerate = true;
    [SerializeField] int mapSize = 25;
    [SerializeField] int regionSize = 2;
    [SerializeField] float tileGenerationSpeed = 0.25f;
    [SerializeField] float regionGenerationSpeed = 0.25f;
    [SerializeField] float scale = 1f;
    [SerializeField] int spawnHeight = 3;

    // TileSets
    [SerializeField] List<Tile> deepOceanTileSet = new List<Tile>();
    [SerializeField] List<Tile> beachTileSet = new List<Tile>();
    [SerializeField] List<Tile> forestTileSet = new List<Tile>();
    [SerializeField] List<Tile> swampTileSet = new List<Tile>();
    [SerializeField] List<Tile> magmaTileSet = new List<Tile>();
    [SerializeField] List<Tile> desertTileSet = new List<Tile>();

    Queue<Region> worldRegions = new Queue<Region>();

    // Drag and Drop Prefabs
    [SerializeField] Slider mapSizeSlider;
    [SerializeField] Slider regionSizeSlider;
    [SerializeField] Slider generationSpeedSlider;
    [SerializeField] Region regionPrefab;
    [SerializeField] Tile spawnTile;
    [SerializeField] Tile spawnTile2;
    bool spawnTwo = false;
    [SerializeField] GameObject mapHolder;

    // Start is called before the first frame update
    void Awake(){}

    void FixedUpdate()
    {
        tileGenerationSpeed = GameObject.Find("GenerationSpeedSlider").GetComponent<Slider>().value;
        tileGenerationSpeed = Mathf.Round(tileGenerationSpeed * 1000f) / 1000f;
        regionGenerationSpeed = GameObject.Find("RegionGenerationSpeedSlider").GetComponent<Slider>().value;
        regionGenerationSpeed = Mathf.Round(regionGenerationSpeed * 1000f) / 1000f;
    }

    public void GenerateNewMap()
    {
        if (canGenerate)
        {
            canGenerate = false;
            foreach (Transform child in mapHolder.transform)
                Destroy(child.gameObject);

            mapSize = (int)GameObject.Find("MapSizeSlider").GetComponent<Slider>().value;
            regionSize = (int)GameObject.Find("RegionSizeSlider").GetComponent<Slider>().value;
            spawnHeight = (int)GameObject.Find("BlockHeightSpawnSlider").GetComponent<Slider>().value;

            scale = regionSize * 0.1f;
            regionPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
            regionPrefab.transform.localScale = scale * regionPrefab.transform.localScale;

            StartCoroutine(RGMap(mapSize, regionSize));
        }
    }

    public IEnumerator RGMap(int mSize, int regSize)
    {
        yield return new WaitForSeconds(0.1f);

        float xSet = 0f;
        float ySet = 0f;

        for (int i = 0; i < mSize; i++)
        {
            for (int j = 0; j < mSize; j++)
            {
                yield return new WaitForSeconds(tileGenerationSpeed);
                Vector3 pos = new Vector3(transform.position.x + ySet, transform.position.y, transform.position.z + xSet);
                worldRegions.Enqueue(Instantiate(regionPrefab, this.transform.position + pos, Quaternion.identity));
                xSet += (scale * 10f);
            }
            xSet = 0f;
            ySet += (scale * 10f);
        }

        xSet = 0f;
        ySet = 0f;

        yield return new WaitForSeconds(2f);

        for (int l = 0; l < mSize; l++)
        {
            for (int k = 0; k < mSize; k++) // QUADRANT
            {
                for (int i = 0; i < regSize; i++)
                {
                    for (int j = 0; j < regSize; j++)
                    {
                        yield return new WaitForSeconds(tileGenerationSpeed);
                        float num = Random.Range(spawnHeight, spawnHeight); // currently same value (can be changed for random effect)

                        Vector3 pos;
                        if (regSize % 2 == 1)
                            pos  = new Vector3(j + (int)(regSize / -2) + ySet, transform.position.y + num, i + (int)(regSize / -2) + xSet);
                        else
                            pos = new Vector3(j + (int)(regSize / -2) + ySet + 0.5f, transform.position.y + num, i + (int)(regSize / -2) + xSet + 0.5f);

                        if (worldRegions.Peek().regionType == Region.Theme.DeepOcean)
                            spawnTile = (Tile)deepOceanTileSet[Random.Range(0, deepOceanTileSet.Count)];
                        if (worldRegions.Peek().regionType == Region.Theme.Forest)
                        {
                            // code theory for next layer / vertical spawns
                            int chance = Random.Range(1, 10); // chance to spawn second layer
                            int x = Random.Range(0, forestTileSet.Count); // choose random tile from tileset
                            spawnTile = (Tile)forestTileSet[x];

                            foreach(Tile tile in forestTileSet)
                            {
                                if(tile.tileType == Tile.Type.Grass && chance <= 2) // 20%
                                {
                                    spawnTwo = true;
                                    spawnTile2 = (Tile)forestTileSet[x];
                                }
                            }
                        }
                        if (worldRegions.Peek().regionType == Region.Theme.Beach)
                            spawnTile = (Tile)beachTileSet[Random.Range(0, beachTileSet.Count)];
                        if (worldRegions.Peek().regionType == Region.Theme.Swamp)
                            spawnTile = (Tile)swampTileSet[Random.Range(0, swampTileSet.Count)];
                        if (worldRegions.Peek().regionType == Region.Theme.Magma)
                            spawnTile = (Tile)magmaTileSet[Random.Range(0, magmaTileSet.Count)];
                        if (worldRegions.Peek().regionType == Region.Theme.Desert)
                            spawnTile = (Tile)desertTileSet[Random.Range(0, desertTileSet.Count)];

                        Instantiate(spawnTile, this.transform.position + pos, Quaternion.identity, mapHolder.transform);

                        // More theory / test code for possible spawn heights
                        //if(spawnTwo)
                        //{
                        //    //Instantiate(spawnTile, this.transform.position + pos, Quaternion.identity, mapHolder.transform);
                        //    Instantiate(spawnTile2, this.transform.position + pos, Quaternion.identity, mapHolder.transform);
                        //    spawnTwo = false;
                        //}
                    }
                }
                Destroy(worldRegions.Dequeue().gameObject);
                xSet += (scale * 10f);
                yield return new WaitForSeconds(regionGenerationSpeed);
            }
            xSet = 0f;
            ySet += (scale * 10f);
        }
        canGenerate = true;
    }
}
