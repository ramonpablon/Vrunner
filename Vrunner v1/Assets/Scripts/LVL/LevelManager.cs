using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instace { get; set; }

    //gameplay
    public bool somenteTransicoes = false;
    [System.NonSerialized]
    public bool SHOW_COLLIDER = false; // mostra as formas primitivas dos objetos 


    // level spawning
    private const float DISTANCE_BEFORE_SPAWN = 96; // quantos metros a frente do player vai instanciar os objetos
    private const int INITIAL_SEGMENTS = 2;
    private const int INITIAL_TRANSITIONS = 1;
    private const int MAX_SEGMENTS_ON_SCREEN = 8;
    private Transform cameraContainer;
    private int segmentsWithoutFreFall;
    private int amountOfSegments;

    [HideInInspector]
    public int continiousSegments;

    private int currentSpawnZ;
    private int y1, y2, y3;
    
    
    // lista de objetos graficos do mapa
    public List<Piece> plaquetas = new List<Piece>();
    public List<Piece> gorduras = new List<Piece>();

    [HideInInspector]
    public List<Piece> pieces = new List<Piece>(); // todos os tipos de objetos graficos


    // lista de segmentos
    public List<Segment> avaliableSegments = new List<Segment>();
    public List<Segment> avaliableTransitions = new List<Segment>();

    [HideInInspector]
    public List<Segment> segments = new List<Segment>();


    // Use this for initialization
    void Awake () {
        Instace = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
	}
    
    private void Start()
    {
        for(int i = 0; i < INITIAL_SEGMENTS; i++)
        {
            if (i < INITIAL_TRANSITIONS)
                SpawnTransitions();
            else
                GenerateSegments();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            somenteTransicoes = true;
        if (Input.GetKeyDown(KeyCode.Z))
            somenteTransicoes = false;

        if(currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
        {
            GenerateSegments();
        }
        if(amountOfSegments >= MAX_SEGMENTS_ON_SCREEN)
        {
            segments[amountOfSegments - 1].Despawn();
            amountOfSegments--;
        }

    }

    private void GenerateSegments()
    {
		if(Random.Range(0,1f) < (continiousSegments * 0.25f) || somenteTransicoes) // no maximo 5 segmentos sem ter um espaço vazio de respiro
        {
            continiousSegments = 0;
            SpawnTransitions(); // spawna uma transição
        }
        else
		{
            continiousSegments++;
            SpawnSegments(); // spawna um segmento alheatório
		}
    }

    private void SpawnSegments()
    {
        List<Segment> possibleSeg = avaliableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);

        int id = Random.Range(0, possibleSeg.Count);

        Segment s = GetSegment(id, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform); // puramente para fins de organização
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.lenght;
        segmentsWithoutFreFall++;
        amountOfSegments++;
        s.Spawn(); // deixa visivel o objeto encontrado
    }
    
    private void SpawnTransitions()
    {
        List<Segment> possibleTransition = avaliableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleTransition.Count);

        Segment s = GetSegment(id, true);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.lenght;
        amountOfSegments++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if(s == null)
        {
            // se transition for true, mandara uma transição da ID que eu mandar, se false manda um segmento
            GameObject go = Instantiate((transition) ? avaliableTransitions[id].gameObject : avaliableSegments[id].gameObject) as GameObject;

            s = go.GetComponent<Segment>();


            // manda o tipo de seguimentos que desejo colocar na tela
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
    
    public Piece GetPieces(PieceType pieceType, int visualIndex) // seta os objetos graficos no jogo
    {
        Piece p = pieces.Find(x => x.type == pieceType && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if(p == null)
        {
            GameObject go = null;
            if (pieceType == PieceType.gordura)
                go = gorduras[visualIndex].gameObject;
            else if (pieceType == PieceType.plaqueta)
                go = plaquetas[visualIndex].gameObject;

            go = Instantiate(go); // instancia apenas uma fez os componentes no cenário
            p = go.GetComponent<Piece>();
            pieces.Add(p); // e adiciona-os na lista pra só busca-los depois
        }

        return p;
    }
}