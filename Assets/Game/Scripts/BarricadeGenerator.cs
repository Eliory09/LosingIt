using UnityEngine;
using Random = UnityEngine.Random;

public class BarricadeGenerator : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject[] barricades;
    [SerializeField] private int maxIndex;
    [SerializeField] private float maxRandX = 3f;
    [SerializeField] private int spawnerDistance = 3;
    [SerializeField] public float timeToGenerate = 5;

    [SerializeField] private SpawnerScript spawner;
    [SerializeField] private GameObject ball;
    private float _timer;
    private int _lastXPose;
    private static bool _initiateOutSide = false;

    #endregion

    #region MonoBehaviour

    private void Update()
    {
        if (!_initiateOutSide) return;
        _initiateOutSide = false;
        SquareInstantiate(barricades[0]);
    }

    #endregion

    #region Methods

    public static void CallToInitiate()
    {
        _initiateOutSide = true;
    }

    /**
     * Instantiating a new barricade.
     */
    private void SquareInstantiate(GameObject barricade)
    {
        var spawnerPos = spawner.transform.position;
        var xPos = Mathf.RoundToInt(ball.transform.position.x);
        if (xPos == _lastXPose)
        {
            xPos = Mathf.RoundToInt(Random.Range(xPos - maxRandX, xPos + maxRandX));
        }

        _lastXPose = xPos;

        var yPos = Mathf.RoundToInt(spawnerPos.y + spawnerDistance);
        var newBarricadePos = new Vector3(xPos, yPos, 0);
        Instantiate(barricade, newBarricadePos, Quaternion.identity);
    }

    #endregion
}