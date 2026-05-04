using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] GrassManager gm;
    [SerializeField] Rigidbody rb;
    float time = 0;
    float timef = 0;

    // -- click メンバ --

    // -- d&d メンバ --

    // -- grazing メンバ --
    bool hunger = false;
    public int AppetiteLevel { get; set; } = 1;
    float grazingInterval = 10f;

    // -- wandering メンバ --
    // 動いたあと止まってる時間。テキトー
    float[] wanderingInterval = { 5f, 10f, 4f, 13f};
    int wiIndex = 0;
    public int SpeedLevel{ get; set; } = 1;
    float speed = 5;
    Vector3 destination;

    void Start()
    {
        if (gm == null)
            gm = GameObject.FindGameObjectWithTag("GrassManager").GetComponent<GrassManager>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        grazingInterval = 10f + AppetiteLevel * 15f;
        speed = 0.7f + SpeedLevel * 0.3f;

    }

    void Update()
    {
        if(!hunger) return;
        
        time += Time.deltaTime;
        if (IsEatable() && !IsWandering())
        {
            // 下のブロックに草が生えているかを判定する関数をgrassManagerに作る
            // なくてもtime = 0;
            Grazing();
        }
    }

    void FixedUpdate()
    {
        timef += Time.deltaTime;

        if (IsWandering())
        {
            Wandering();
        }

    }

    bool IsEatable()
    {
        return time > grazingInterval;
    }

    void Grazing()
    {
        gm.Grazing(transform.position);
        hunger = false;
        time = 0;
    }

    bool IsWandering()
    {
        return timef > wanderingInterval[wiIndex];
    }

    // うろうろ
    void Wandering()
    {
        /*
        いい感じにピクミンみたいに動かす
        */
        rb.position = Vector3.MoveTowards(rb.position, destination, speed * Time.deltaTime);
        if (Vector3.SqrMagnitude(rb.position - destination) < 0.4f)
        {
            Arrived();
        }
    }

    void Arrived()
    {
        timef = 0;
        int x = Random.Range(0, gm.RanchSize);
        int z = Random.Range(0, gm.RanchSize);
        destination = new Vector3(x, rb.position.y, z);
        wiIndex++;
        if (wiIndex >= wanderingInterval.Length)
        {
            wiIndex = 0;
        }
    }

    // Merge後にGameMasterなどから呼ばれる想定
    public void SetHunger(bool b)
    {
        hunger = b;
    }
}
