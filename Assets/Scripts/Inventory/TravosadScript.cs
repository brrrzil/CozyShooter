using UnityEngine;
using Cinemachine;
using System.Collections;

public class TravosadScript : MonoBehaviour, IWeapon
    // Оружие "Травосад" в разработке. Оно будет рисовать кистью "Трава" на террейне
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Настройки кисти")]
    public float maxDistance = 50f;
    public float brushSize = 5f;
    public float brushStrength = 0.3f;

    [Header("Настройки травы")]
    public int grassPrototypeIndex = 0;
    public LayerMask terrainLayer = 1;

    private Terrain terrain;
    private TerrainData terrainData;
    [SerializeField] private Camera mainCamera;
    [SerializeField] GameObject prefab;

    void Start()
    {        
        // Находим Terrain в сцене
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
        // Подсказка на экране для проверяющих
        StartCoroutine(ShowTip());

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, terrainLayer))
        {
            if (hit.collider is TerrainCollider)
            {
                PaintGrassAtPosition(hit.point);
            }
        }
    }

    void PaintGrassAtPosition(Vector3 worldPosition)
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
        int brushSizeInt = (int)brushSize;

        // Получаем текущий слой деталей
        int[,] detailLayer = terrainData.GetDetailLayer(
            Mathf.Max(0, detailX - brushSizeInt / 2),
            Mathf.Max(0, detailY - brushSizeInt / 2),
            brushSizeInt, brushSizeInt, grassPrototypeIndex
        );

        Instantiate(prefab, worldPosition, new Quaternion().normalized);

        // Рисуем круговую кисть
        for (int x = 0; x < brushSizeInt; x++)
        {
            for (int y = 0; y < brushSizeInt; y++)
            {
                // Вычисляем расстояние от центра кисти
                float distance = Vector2.Distance(
                    new Vector2(x, y),
                    new Vector2(brushSizeInt * 0.5f, brushSizeInt * 0.5f)
                );

                // Если точка внутри радиуса кисти - добавляем траву
                if (distance < brushSizeInt * 0.5f)
                {
                    float strength = 1 - (distance / (brushSizeInt * 0.5f));
                    detailLayer[x, y] = Mathf.Min(
                        detailLayer[x, y] + (int)(strength * brushStrength * 255),
                        255
                    );
                }
            }
        }

        // Применяем изменения к Terrain
        terrainData.SetDetailLayer(
            Mathf.Max(0, detailX - brushSizeInt / 2),
            Mathf.Max(0, detailY - brushSizeInt / 2),
            grassPrototypeIndex,
            detailLayer
        );
    }

    // Проверяющим
    [SerializeField] GameObject tip;

    private IEnumerator ShowTip()
    {
        tip.SetActive(true);
        yield return new WaitForSeconds(5);
        tip.SetActive(false);
    }
}
