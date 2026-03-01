using UnityEngine;
using System.Collections.Generic;

public class SpwanManager : MonoBehaviour
{
    [Header("MaxDistance")]
    [SerializeField] private float maxMinusYCoordinate;

    [Header("FishesData")]
    [SerializeField] private List<FishData> fishList = new List<FishData>();
    [SerializeField] private int maxFishs;

    #region BiomData
    [Header("BiomData")]
    [SerializeField] private float maxBiomeRange;
    #endregion 

    // private data
    private Vector2 startPosition = new Vector2(0, 0);
    private float currentPosition;

    #region Start
    private void Start()
    {
        SpawnFishs();
    }
    #endregion

    #region StartToSapwnFish
    private void SpawnFishs() {

        for (int i = 0; i< maxFishs; i++) {
            
            float randomY = Random.Range(1f, maxMinusYCoordinate);
            float randomX = Random.Range(-5f, 5f);
            SpawnRandomFish(randomY, randomX);
        }
    }
    #endregion

    #region SpawnRandomFish
    private void SpawnRandomFish(float randomY, float randomX)
    {
        int randomFish = Random.Range(0, fishList.Count);
        GameObject newFish;
        MoveFishStandart fishController;
        newFish = fishList[randomFish].TypeOfFish.gameObject;
        GameObject newFishSpawn = Instantiate(newFish, new Vector3(randomX, randomY, 0), Quaternion.Euler(0, 90, 0));
        fishController = newFish.GetComponent<MoveFishStandart>();
        fishController.SkinnedMeshRenderer.material = fishList[randomFish].material[UnityEngine.Random.Range(0, fishList[randomFish].material.Count)];
    }
    #endregion

    #region ClassFishData
    [System.Serializable]
    private class FishData
    {
        public MoveFishStandart TypeOfFish = new MoveFishStandart();
        public List<Material> material = new List<Material>();
    }
    #endregion
}
