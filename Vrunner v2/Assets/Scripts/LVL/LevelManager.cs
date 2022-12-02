using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    //debug
    public bool show_Collider = false;
    public bool only_Transitions = false;
    Vector3 lastRot;

    public static LevelManager Instance { get; set; }


    // level spawning
    private const float DISTANCE_BEFORE_SPAWN = 400.0f;
    private const int INITIAL_SEGMENTS = 5;
    private const int INITIAL_TRANSITIONS = 3;
    private const int MAX_SEGMENTS_ON_SCREEN = 8;

    private int amountOfActiveSegments;
    private int continiousSegments;
    private int currentSpawnZ;


    // lista de pieces
    public List<Piece> gorduras = new List<Piece>();
    public List<Piece> plaquetas = new List<Piece>();
    [HideInInspector]
    public List<Piece> pieces = new List<Piece>(); // todos os pieces

    //lista de segments
    public List<Segment> avaliableSegments = new List<Segment>();
    public List<Segment> avaliableTransitions = new List<Segment>();
    [HideInInspector]
    public List<Segment> segments = new List<Segment>(); // todos os segmentos ativos


    private void Awake()
    {
        Instance = this;
        currentSpawnZ = 0;
    }

    private void Start()
    {
        for (int i = 0; i < INITIAL_SEGMENTS; i++)
        {
            if (i < INITIAL_TRANSITIONS)
                SpawnTransition();
            else
                GenerateSegments();
        }
    }

    private void Update()
    {
        if (currentSpawnZ + transform.position.z < DISTANCE_BEFORE_SPAWN)
            GenerateSegments();

        if(amountOfActiveSegments >= MAX_SEGMENTS_ON_SCREEN)
        {
            segments[amountOfActiveSegments - 1].DeSpawn();
            amountOfActiveSegments--;
        }
    }

    private void GenerateSegments()
    {
        //randomicamente spawna transições, mas não permite mais que 5 segmentos continuos sem ter um respiro de transição
        if (Random.Range(0f, 1f) < (continiousSegments * 0.25f) || only_Transitions)
        {
            continiousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continiousSegments++;
            SpawnSegment();
        }
    }

    private void SpawnSegment()
    {
        int id = Random.Range(0, avaliableSegments.Count);

        Segment s = GetSegment(id, false);

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        if (GameManager.Instance.gameMode == GameMode.cardiovascular)
            s.transform.rotation = MobileInput.Instance.mainAccRotation.transform.rotation * Quaternion.Euler(lastRot);

        lastRot.z -= 72;

        currentSpawnZ += s.lenght;
        amountOfActiveSegments++;
        s.Spawn();
    }
    private void SpawnTransition()
    {
        int id = Random.Range(0, avaliableTransitions.Count);

        Segment s = GetSegment(id, true);

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        if (GameManager.Instance.gameMode == GameMode.cardiovascular)
            s.transform.rotation = MobileInput.Instance.mainAccRotation.transform.rotation * Quaternion.Euler(lastRot);

        lastRot.z -= 72;

        currentSpawnZ += s.lenght;
        amountOfActiveSegments++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;

        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if(s == null)
        {
            // se transition == true, instancia transições, se não, segmentos
            GameObject go = Instantiate((transition) ? avaliableTransitions[id].gameObject : avaliableSegments[id].gameObject) as GameObject;

            s = go.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;

            segments.Insert(0, s);
        }
        else
        {
            // uma forma de saber qual foi o ultimo segmento pego no jogo
            segments.Remove(s);
            segments.Insert(0, s);
        }
        return s;
    }
    public Piece GetPiece(PieceType pieceType, int visualIndex)
    {
        // pega o piece que queremos que estiver em cena mas não esta ativo
        Piece p = pieces.Find(x => x.type == pieceType && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if(p == null)
        {
            // se não tiver nem um piece com as caracteristicas, instancie ele no jogo

            GameObject go = null;
            if (pieceType == PieceType.gordura)
                go = gorduras[visualIndex].gameObject;
            else if (pieceType == PieceType.plaquetas)
                go = plaquetas[visualIndex].gameObject;

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }

        return p;
    }
}
