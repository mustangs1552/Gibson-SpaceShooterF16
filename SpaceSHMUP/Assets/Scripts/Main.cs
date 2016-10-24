// (Unity3D) New monobehaviour script that includes regions for common sections, and supports debugging.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    #region GlobalVareables
    #region DefaultVareables
    public bool isDebug = false;
    private string debugScriptName = "Main";
    #endregion

    #region Static
    public static Main S;
    public static Dictionary<WeaponType, WeaponDefinition> W_DEFS;
    #endregion

    #region Public
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = .5f;
    public float enemySpawnPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp = null;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.Blaster,
        WeaponType.Blaster,
        WeaponType.Spread,
        WeaponType.Shield
    };

    [Header("For debug view only")]
    public WeaponType[] activeWeaponTypes;
    public float enemySpawnRate = 0;
    #endregion

    #region Private

    #endregion
    #endregion

    #region CustomFunction
    #region Static
    public static WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (W_DEFS.ContainsKey(wt)) return W_DEFS[wt];
        return new WeaponDefinition();
    }
    #endregion

    #region Public
    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;

        Vector3 pos = Vector3.zero;
        float xMin = Utils.CamBounds.min.x + enemySpawnPadding;
        float xMax = Utils.CamBounds.max.x - enemySpawnPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = Utils.CamBounds.max.y + enemySpawnPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", enemySpawnRate);
    }

    public void ShipDestroyed(Enemy e)
    {
        if(Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);

            pu.transform.position = e.transform.position;
        }
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("scene0");
    }
    #endregion

    #region Private

    #endregion

    #region Debug
    private void PrintDebugMsg(string msg)
    {
        if (isDebug) Debug.Log(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    private void PrintWarningDebugMsg(string msg)
    {
        Debug.LogWarning(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    private void PrintErrorDebugMsg(string msg)
    {
        Debug.LogError(debugScriptName + "(" + this.gameObject.name + "): " + msg);
    }
    #endregion

    #region Getters_Setters

    #endregion
    #endregion

    #region UnityFunctions

    #endregion

    #region Start_Update
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        PrintDebugMsg("Loaded.");

        S = this;

        Utils.SetCameraBounds(this.GetComponent<Camera>());
        enemySpawnRate = 1f / enemySpawnPerSecond;
        Invoke("SpawnEnemy", enemySpawnRate);

        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions) W_DEFS[def.type] = def;
    }
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for (int i = 0; i < weaponDefinitions.Length; i++) activeWeaponTypes[i] = weaponDefinitions[i].type;
    }
    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {

    }
    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {

    }
    // LateUpdate is called every frame after all other update functions, if the Behaviour is enabled.
    void LateUpdate()
    {

    }
    #endregion
}