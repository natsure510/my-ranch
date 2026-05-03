using UnityEngine;

public class CreatureInfo
{
    public CreatureInfo(int id, string name, CreatureStatus status)
    {
        Id = id;
        Name = name;
        Status = status;
        // 誕生日などの情報も入る
    }

    public int Id { get; }
    public string Name { get; }
    public CreatureStatus Status { get; }
}

public struct CreatureStatus
{

    // 最終的にこっちにしてInheritanceは消す予定
    public CreatureStatus(
    int speed, 
    int appetite,
    Color color, 
    int size, 
    int attackPoints, 
    int deffensePoints)
    {
        Speed = speed;
        Appetite = appetite;
        Inheritance = new CreatureInheritanceStatus(color, size, attackPoints, deffensePoints);
        Color = color;
        Size = size;
        AttackPoints = attackPoints;
        DeffensePoints = deffensePoints;
    }

    public CreatureStatus(int speed, int appetite, CreatureInheritanceStatus inheritanceStatus)
    {
        Speed = speed;
        Appetite = appetite;
        Inheritance = inheritanceStatus;
        Color = inheritanceStatus.Color;
        Size = inheritanceStatus.Size;
        AttackPoints = inheritanceStatus.AttackPoints;
        DeffensePoints = inheritanceStatus.DeffensePoints;
    }

    public int Speed { get; }
    public int Appetite { get; }
    public CreatureInheritanceStatus Inheritance { get; }
    public Color Color { get; }
    public int Size { get; }
    public int AttackPoints { get; }
    public int DeffensePoints { get; }
}

public struct CreatureInheritanceStatus
{
    public CreatureInheritanceStatus(Color color, int size, int attackPoints, int deffensePoints)
    {
        Color = color;
        Size = size;
        AttackPoints = attackPoints;
        DeffensePoints = deffensePoints;
    }

    public Color Color { get; }
    public int Size { get; }
    public int AttackPoints { get; }
    public int DeffensePoints { get; }
}