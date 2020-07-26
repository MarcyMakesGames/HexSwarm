﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAVController : MonoBehaviour
{
    [SerializeField] protected GameObject worldUnitPrefab;
    [SerializeField] protected AudioClip[] initMoveSFX;
    [SerializeField] protected AudioClip[] initAttackSFX;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float dieSpeed;
    
    protected GameManager gameManager;
    protected ConfigManager configManager;
    protected Dictionary<IUnit, GameObject> worldUnits = new Dictionary<IUnit, GameObject>();
    protected Dictionary<Queue<Vector3>, GameObject> worldUnitPath = new Dictionary<Queue<Vector3>, GameObject>();
    protected Dictionary<Units, AudioClip> movementSounds = new Dictionary<Units, AudioClip>();
    protected Dictionary<Units, AudioClip> attackSounds = new Dictionary<Units, AudioClip>();
    protected List<GameObject> totalUnitsToDie = new List<GameObject>();
    protected List<GameObject> currentUnitsToDie = new List<GameObject>();

    public void PlaceNewUnit(IUnit unit)
    {
        GameObject worldUnit = Instantiate(worldUnitPrefab, this.transform);
        SpriteRenderer renderer = worldUnit.GetComponent<SpriteRenderer>();

        if (!worldUnits.ContainsKey(unit))
            worldUnits.Add(unit, worldUnit);

        renderer.color = configManager.TeamColors[unit.Team.TeamNumber].PrimaryColor;
        renderer.sprite = unit.Icon;
        Debug.Log(unit.Team.TeamNumber);
        
        worldUnit.transform.position = GameManager.Battlefield.GetWorldLocation(unit.Location);
    }

    public void MoveUnit(IUnit unit, IEnumerable<Vector3Int> path)
    {
        if (unit == null)
            return;
        GameObject worldUnit;
        worldUnits.TryGetValue(unit, out worldUnit);

        Queue<Vector3> worldPath = new Queue<Vector3>();

        foreach (Vector3Int location in path)
        {
            ICell cell;
            GameManager.Battlefield.World.TryGetValue(location, out cell);
            worldPath.Enqueue(cell.WorldPosition);
        }
        
        worldUnitPath.Add(worldPath, worldUnit);
    }

    public void DestroyUnit(IUnit unit)
    {
        GameObject deadUnit;
        worldUnits.TryGetValue(unit, out deadUnit);
        totalUnitsToDie.Add(deadUnit);

        worldUnits.Remove(unit);
    }

    public void ChangeTeamColors(Dictionary<Teams, ColorConfig> colors)
    {
        foreach(KeyValuePair<IUnit, GameObject> worldUnit in worldUnits)
            worldUnit.Value.GetComponent<SpriteRenderer>().color = colors[worldUnit.Key.Team.TeamNumber].PrimaryColor;
    }

    public void Nuke()
    {
        foreach(KeyValuePair<IUnit, GameObject> entry in worldUnits)
        {
            Destroy(entry.Value);
        }

        worldUnits.Clear();
        worldUnitPath.Clear();
        totalUnitsToDie.Clear();
        currentUnitsToDie.Clear();
    }

    protected void Awake()
    {
        if (configManager == null)
            configManager = FindObjectOfType<ConfigManager>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    protected void Update()
    {
        if (worldUnitPath.Count > 0)
        {
            GameObject worldUnit = worldUnitPath.First().Value;
            Vector3 nextPosition = worldUnitPath.First().Key.Peek();
            worldUnit.transform.position = Vector3.MoveTowards(
                    worldUnit.transform.position,
                    nextPosition,
                    moveSpeed * Time.deltaTime);

            KillCurrentUnits(worldUnit);

            if (worldUnit.transform.position == nextPosition)
                nextPosition = worldUnitPath.First().Key.Dequeue();
            

            if (worldUnitPath.First().Key.Count == 0 && worldUnit.transform.position == nextPosition)
                worldUnitPath.Remove(worldUnitPath.First().Key);
        }

        if (worldUnitPath.Count == 0)
            KillAllUnits();
    }

    protected void KillAllUnits()
    {
        if(totalUnitsToDie.Count > 0)
            foreach(GameObject deadUnit in totalUnitsToDie)
            {
                SpriteRenderer sprite = deadUnit.GetComponent<SpriteRenderer>();

                Color targetAlpha = new Color(sprite.color.r, sprite.color.g, sprite.color.r, sprite.color.a);
                targetAlpha.a = Mathf.Clamp01(targetAlpha.a - (dieSpeed * Time.deltaTime));
                sprite.color = targetAlpha; 

                if(sprite.color.a == 0)
                {
                    totalUnitsToDie.Remove(deadUnit);
                    Destroy(deadUnit);
                    break;
                }
            }
    }

    protected void KillCurrentUnits(GameObject worldUnit)
    {
        foreach (GameObject deadUnit in totalUnitsToDie)
        {
            if (worldUnit.transform.position == deadUnit.transform.position)
            {
                currentUnitsToDie.Add(deadUnit);
                totalUnitsToDie.Remove(deadUnit);
                break;
            }
        }

        if (currentUnitsToDie.Count > 0)
            foreach (GameObject deadUnit in currentUnitsToDie)
            {
                SpriteRenderer sprite = deadUnit.GetComponent<SpriteRenderer>();

                Color targetAlpha = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(1, 0, dieSpeed * Time.deltaTime));
                sprite.color = targetAlpha;

                if (sprite.color.a == 0)
                {
                    currentUnitsToDie.Remove(deadUnit);
                    Destroy(deadUnit);
                }
            }
    }

    protected void PlayMoveSFX(IUnit unit)
    {
        configManager.PlaySound(movementSounds[gameManager.UnitManager[unit.ID].ID]);
    }

    protected void PlayAttackSFX(IUnit unit)
    {
        configManager.PlaySound(attackSounds[gameManager.UnitManager[unit.ID].ID]);
    }

    [ContextMenu("Play Attack Sound 1")]
    protected void TestPlaySFX()
    {
        configManager.PlaySound(initAttackSFX[0]);
    }
}