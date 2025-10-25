using UnityEngine;
using Cinemachine;

public class CvetostrelScript : MonoBehaviour, IWeapon
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Camera mainCamera;

    [Header("Настройки кисти")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float brushSize = 5f;

    [Header("Настройки цветов")]
    [SerializeField] GameObject[] flowers;
    [SerializeField] private LayerMask terrainLayer = 1;

    private Terrain terrain;
    private TerrainData terrainData;

    void Start()
    {
        //Находим Terrain в сцене
        terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            terrainData = terrain.terrainData;
        }
        else
        {
            Debug.LogError("Terrain не найден в сцене!");
        }
    }

    public void Shoot()
    {
        // Выстрел лучом из камеры в террейн
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, terrainLayer))
        {
            if (hit.collider is TerrainCollider)
            {
                FlowerShot(hit.point);
            }
        }
    }

    void FlowerShot(Vector3 worldPosition)
    {
        // Конвертируем мировые координаты в координаты Terrain
        Vector3 terrainLocalPos = worldPosition - terrain.transform.position;
        Vector3 normalizedPos = new Vector3(
            terrainLocalPos.x / terrainData.size.x,
            terrainLocalPos.y / terrainData.size.y,
            terrainLocalPos.z / terrainData.size.z
        );

        // Рассчитываем координаты для деталей (травы)
        int detailX = (int)(normalizedPos.x * terrainData.detailWidth);
        int detailY = (int)(normalizedPos.z * terrainData.detailHeight);

        // Создаём случайный цветок
        Instantiate(RandomFlower(), worldPosition, new Quaternion().normalized);
    }

    private GameObject RandomFlower()
    {
        int flowerNumber = Random.Range(0, flowers.Length);

        GameObject sourceFlower = flowers[flowerNumber];
        GameObject returnFlower = sourceFlower;

        return returnFlower;
    }
}