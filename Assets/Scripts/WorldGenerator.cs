using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    private class World
    {
        public int Height = 4;
        public int Width = 4;

        public bool PitAt(Position p)
        {
            if ((p.X == 3 && p.Y == 3) || (p.X == 2 && p.Y == 2) || (p.X == 2 && p.Y == 0)) return true;
            return false;
        }

        public bool WumpusAt(Position p)
        {
            if (p.X == 0 && p.Y == 2)
                return true;
            return false;

        }

        public bool GoldAt(Position p)
        {
            if (p.X == 1 && p.Y == 2) return true;
            return false;
        }
    }

    private struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    private World world;
    public GameObject PlatformPrefab;
    public GameObject WorldGameObject;

    public float YPosition = 5.35f;

    private Dictionary<string, GameObject> WumpusPrefabs;
    private Dictionary<string, AudioClip> WumpusSounds;
    private Agent _agent;

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

    public AudioSource AudioSrc;

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
        AudioSrc = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        world = new World();
        CreateWorldPlatform();
        _agent = Instantiate(WumpusPrefabs["Agent"], new Vector3(0, YPosition, 0), Quaternion.Euler(0, 180f, 0)).GetComponent<Agent>();
	}

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            MoveAgent(_agent.transform.position + new Vector3(0, 0, 1));
            AudioSrc.clip = WumpusSounds["Move"];
            AudioSrc.Play();
        }
    }

    void MoveAgent(Vector3 newAgentPos, float moveDuration = 1f)
    {
        _agent.SetLerpPos(newAgentPos, moveDuration);
    }

    void CreateWorldPlatform()
    {
        for (int i = 0; i < world.Height; i++)
        {
            for (int j = 0; j < world.Width; j++)
            {
                var platform = Instantiate(PlatformPrefab, new Vector3(i, 0, j), Quaternion.identity, WorldGameObject.transform);
                platform.name = $"({i},{j})";
                var pos = new Position(i, j);

                if (world.GoldAt(pos))
                    Instantiate(WumpusPrefabs["Treasure"], new Vector3(i, YPosition, j), Quaternion.Euler(0, 180f, 0));
                else if (world.PitAt(pos))
                    Instantiate(WumpusPrefabs["Pit"], new Vector3(i, YPosition, j), Quaternion.Euler(0, 180f, 0));
                else if (world.WumpusAt(pos))
                    Instantiate(WumpusPrefabs["Wumpus"], new Vector3(i, YPosition, j), Quaternion.Euler(0, 180f, 0));
            }
        }
    }
}
