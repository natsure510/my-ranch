using System;
using UnityEngine;

public class CreateCreature : MonoBehaviour
{
    // 生物合成・作成の処理と画面の表示などを行う
    // 完成後は完成した生物とそのステータス表示。画面を閉じると牧場中央にドロップ

    private GameObject prefab;

    private int newerID = 0;

    void Start()
    {
        newerID = 0; // GameMasterから入れて都度更新
    }

    public void CreateNewCreature(string name, CreatureInheritanceStatus inheritanceStatus, Action<CreatureInfo> action)
    {
        newerID++;
        
        CreatureInfo info = new CreatureInfo(newerID, name, GetRandomStatus(inheritanceStatus));
        CreateGameObject(info);
        // GameMaster から Callback を入れる
        action(info);
    }

    CreatureStatus GetRandomStatus(CreatureInheritanceStatus inheritanceStatus)
    {
        int speed = UnityEngine.Random.Range(0,5);
        int appetite = UnityEngine.Random.Range(0,5);
        // プレイヤーがステ振りする inheritance
        //int attackPoints = UnityEngine.Random.Range(0,5);
        //int deffensePoints = UnityEngine.Random.Range(0,5);
        return new CreatureStatus(speed, appetite, inheritanceStatus);
    }

    public void MergeCreature(CreatureInfo parent1, CreatureInfo parent2, Action<CreatureInfo> action)
    {
        newerID++;
        string name = parent1.Name + parent2.Name; // このままだと延々と名前が長くなる

        int speed = UnityEngine.Random.Range(0,5);
        int appetite = UnityEngine.Random.Range(0,5);
        Color color = Color.Lerp(parent1.Status.Color, parent2.Status.Color, 0.5f);
        int size = GetChildSize(parent1.Status.Size, parent2.Status.Size);
        // こういうただ平均取るだけのステータス他にも増えそうだから、
        // 配列にして各ステの名前配列も作りそれを参照表示するようにしてもいい（テーブル）
        int attackPoints = AveragePlusOne(parent1.Status.AttackPoints, parent2.Status.AttackPoints);
        int deffensePoints = AveragePlusOne(parent1.Status.DeffensePoints, parent2.Status.DeffensePoints);

        CreatureInheritanceStatus inheritanceStatus = new CreatureInheritanceStatus(color, size, attackPoints, deffensePoints);
        CreatureInfo creatureInfo = new CreatureInfo(newerID, name, new CreatureStatus(speed, appetite, inheritanceStatus));
        // Info は GameMasterに渡す
        CreateGameObject(creatureInfo);
        action(creatureInfo);
    }

    void CreateGameObject(CreatureInfo info)
    {
        GameObject obj = Instantiate(prefab);
        obj.GetComponent<Material>().color = info.Status.Color;
        float s = info.Status.Size;
        obj.transform.localScale = new Vector3(s, s, s);
        // status をそのまんま渡して向こうで加工する方が疎結合なのでは？
        obj.GetComponent<Creature>().AppetiteLevel = info.Status.Appetite;
        obj.GetComponent<Creature>().SpeedLevel = info.Status.Speed;
    }

    int GetChildSize(int sizeA, int sizeB)
    {
        return Mathf.Clamp((sizeA + sizeB) / 2 + UnityEngine.Random.Range(-3, 4), 1, 20);
    }

    int AveragePlusOne(int a, int b)
    {
        return (a + b) / 2 + 1;
    }


    // CreateNewCreatureの時にspeedとappetiteを外から決めれるコールバック作る
}
