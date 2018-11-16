using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    private CaveWorld world;
    public GameObject PlatformPrefab;
    public GameObject DoorPrefab;
    public GameObject WorldGameObject;

    public float YPosition = 5.35f;

    [SerializeField]
    private List<Position> WumpusPositions;
    [SerializeField]
    private List<Position> PitPositions;
    [SerializeField]
    private Position GoldPosition;

    private Dictionary<string, GameObject> WumpusPrefabs;
    private Dictionary<string, AudioClip> WumpusSounds;
    private Agent _agent;
    private GameObject Treasure;
    private bool _gameRunning = true;

    [Serializable]
    public struct WumpusObject
    {
        public string Name;
        public GameObject Prefab;
    }
    [Serializable]
    public struct SoundEffect
    {
        public string Name;
        public AudioClip Sound;
    } 
    public WumpusObject[] ObjectPrefabs;
    public SoundEffect[] SoundEffects;

    public AudioSource MoveAudioSrc;
    public AudioSource EffectsAudioSrc;

    private void Awake()
    {
        WumpusPrefabs = new Dictionary<string, GameObject>();
        foreach (var pfb in ObjectPrefabs)
        {
            WumpusPrefabs.Add(pfb.Name, pfb.Prefab);
        }
        WumpusSounds = new Dictionary<string, AudioClip>();
        foreach (var pfb in SoundEffects)
        {
            WumpusSounds.Add(pfb.Name, pfb.Sound);
        }
        MoveAudioSrc = GetComponent<AudioSource>();
    }

    private void PlaySound(string soundName, bool specialEffect = true)
    {
        var audioSrc = specialEffect ? EffectsAudioSrc : MoveAudioSrc;

        audioSrc.clip = WumpusSounds[soundName];
        audioSrc.Play();
    }

    // Use this for initialization
    void Start () {
        world = new CaveWorld(WumpusPositions, PitPositions, GoldPosition);
        CreateWorldPlatform();
        _agent = Instantiate(WumpusPrefabs["Agent"], new Vector3(0, YPosition, 0), Quaternion.Euler(0, 180f, 0)).GetComponent<Agent>();
        EffectsAudioSrc = _agent.GetComponent<AudioSource>();

        world.OnBreezePercepted += () =>
        {
            PlaySound("Breeze");
        };
        world.OnMove += (Position p) =>
        {
            var vecPos = new Vector3(p.X, _agent.transform.position.y, p.Y);
            _agent.SetLerpPos(vecPos);
            PlaySound("Move", false);
        };
        world.OnPitEncountered += () =>
        {
            Destroy(_agent);
            PlaySound("Pit");
            _gameRunning = false;
        };
        world.OnStenchPercepted += () => PlaySound("Stench");
        world.OnTreasureEncountered += () =>
        {
            Destroy(Treasure);
            PlaySound("Gold");
        };
        world.OnWumpusEncountered += () =>
        {
            Destroy(_agent);
            PlaySound("Wumpus");
            _gameRunning = false;
        };
        world.OnGoalComplete += () =>
        {
            PlaySound("Goal");
            _gameRunning = false;
        };
    }

    private float UpdateTimer = 0f;
    public float UpdateTimeSecs = 1f;

    private void Update()
    {
        if (!_gameRunning) return;
        if (UpdateTimer > UpdateTimeSecs)
        {
            world.Iterate();
            UpdateTimer = 0f;
        }
        else
            UpdateTimer += Time.deltaTime;
    }

    void MoveAgent(Vector3 newAgentPos, float moveDuration = 1f)
    {
        _agent.SetLerpPos(newAgentPos, moveDuration);
    }

    void CreateWorldPlatform()
    {
        for (int i = 0; i < world.WorldHeight; i++)
        {
            for (int j = 0; j < world.WorldWidth; j++)
            {
                GameObject platform;
                if (i == 0 && j == 0)
                    platform = Instantiate(DoorPrefab, new Vector3(i, 0, j), Quaternion.identity, WorldGameObject.transform);
                else
                    platform = Instantiate(PlatformPrefab, new Vector3(i, 0, j), Quaternion.identity, WorldGameObject.transform);
                platform.name = $"({i},{j})";
                var pos = new Position(i, j);

                if (world.GoldAt(pos))
                    Treasure = Instantiate(WumpusPrefabs["Treasure"], new Vector3(i, YPosition, j), Quaternion.Euler(0, 180f, 0));
                else if (world.PitAt(pos))
                    Instantiate(WumpusPrefabs["Pit"], new Vector3(i, YPosition, j), Quaternion.Euler(0, 180f, 0));
                else if (world.WumpusAt(pos))
                    Instantiate(WumpusPrefabs["Wumpus"], new Vector3(i, YPosition, j), Quaternion.Euler(0, 180f, 0));
            }
        }
    }
}
