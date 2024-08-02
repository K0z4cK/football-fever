using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum StatType {Strength, Speed, Endurance, Accuracy, Teaming};

public class Footballer
{
    private string _id;
    public string Id => _id;

    private Sprite _sprite;
    public Sprite Sprite => _sprite;

    private string _name;
    public string Name => _name;

    private FootballerStats _stats;
    public FootballerStats Stats => _stats;

    private int _points;
    public int Points => _points;

    public float Rating => (_stats.strength + _stats.speed + _stats.endurance + _stats.accuracy + _stats.teaming) / 5f;

    public int SumOfPoints => _stats.strength + _stats.speed + _stats.endurance + _stats.accuracy + _stats.teaming;

    public Footballer()
    {
        Builder();
        //_rating = 1;
    }

    private void Builder()
    {
        _id = IdGenerator.NewId();
        _name = NamesCollection.GetRandomName();

        _stats = new FootballerStats()
        {
            strength = 1,
            speed = 1,
            endurance = 1,
            accuracy = 1,
            teaming = 1
        };
        _points = 7;
    }

    public Footballer(Footballer footballer)
    {
        _id = footballer.Id;
        _name = footballer.Name;
        _stats = footballer.Stats;
        _points = footballer.Points;
        _sprite = footballer.Sprite;
    }

    public Footballer(FootballerData data)
    {
        _id = data.id;
        _name = data.name;
        _stats = data.stats;
        _points = data.points;
        //_sprite = footballer.Sprite;
    }

    public Footballer(Sprite sprite)
    {
        Builder();

        _sprite = sprite;
    }

    public Footballer(FootballerStats stats)
    {
        Builder();

        _stats = stats;
        _points = 0;
    }

    public Footballer(string name, FootballerStats stats)
    {
        Builder();

        _name = name;

        _stats = stats;
        _points = 0;
    }

    public Footballer(string name, FootballerStats stats, Sprite sprite, int points)
    {
        Builder();

        if (name == "")
            name = NamesCollection.GetRandomName();

        _name = name;

        _stats = stats;

        _sprite = sprite;
        _points = points;
    }

    public Footballer(FootballerStats stats, Sprite sprite)
    {
        Builder();

        _stats = stats;

        _sprite = sprite;
        _points = 0;
    }

    public void SetNewName(string name = "")
    {
        if(name == "")
            _name = NamesCollection.GetRandomName();
        else
            _name = name;
    }

    public void SetNewId(string id = "")
    {
        if (id == "")
            _id = IdGenerator.NewId();
        else
            _id = id;
        
    }

    public void SetStatsRandom()
    {
        _stats = new FootballerStats()
        {
            strength = 1,
            speed = 1,
            endurance = 1,
            accuracy = 1,
            teaming = 1
        };

        _points = 0;
        int randomPoints = 10;

        var types = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToList();
        types.Shuffle();

        foreach (var type in types)
        {
            int nextPoints = RandomTools.Random.Next(randomPoints + 1);
            switch (type)
            {
                case StatType.Strength:
                    _stats.strength += nextPoints;
                    break;
                case StatType.Speed:
                    _stats.speed += nextPoints;
                    break;
                case StatType.Endurance:
                    _stats.endurance += nextPoints;
                    break;
                case StatType.Accuracy:
                    _stats.accuracy += nextPoints;
                    break;
                case StatType.Teaming:
                    _stats.teaming += nextPoints;
                    break;
            }
            randomPoints -= nextPoints;
        }
        //_rating = GetStatsResult();
    }

    public void IncreaseStat(StatType type)
    {
        if (_points == 0)
            return;

        switch (type)
        {
            case StatType.Strength:
                _stats.strength += 1;
                break;
            case StatType.Speed:
                _stats.speed += 1;
                break;
            case StatType.Endurance:
                _stats.endurance += 1;
                break;
            case StatType.Accuracy:
                _stats.accuracy += 1;
                break;
            case StatType.Teaming:
                _stats.teaming += 1;
                break;
        }
        _points--;
        //_rating = GetStatsResult();
    }

    private StatType GetBestStat()
    {
        if ( _stats.strength > _stats.endurance && _stats.strength > _stats.accuracy)
            return StatType.Strength;
        else if (_stats.endurance > _stats.strength && _stats.endurance > _stats.accuracy)
            return StatType.Endurance;
        else 
            return StatType.Accuracy;
    }

    public float GetStatsResult()
    {
        StatType bestStat = GetBestStat();

        float result = 0f;

        switch (bestStat)
        {
            case StatType.Strength:
                result = 0.7f * _stats.strength + _stats.accuracy / _stats.speed + 0.2f * _stats.endurance;
                break;
            case StatType.Endurance:
                result = 0.7f * _stats.strength + 0.7f * _stats.accuracy + 0.7f * _stats.speed;
                break;
            case StatType.Accuracy:
                result = _stats.accuracy / _stats.strength + _stats.endurance + 0.1f * _stats.speed;
                break;
        }

        return result;
    }

    public float GetStatsTeamResult()
    {
        StatType bestStat = GetBestStat();

        float result = 0f;

        switch (bestStat)
        {
            case StatType.Strength:
                result = 0.7f * _stats.strength + _stats.accuracy / _stats.speed + 0.2f * _stats.endurance;
                break;
            case StatType.Endurance:
                result = 0.7f * _stats.strength + 0.7f * _stats.accuracy + 0.7f * _stats.speed;
                break;
            case StatType.Accuracy:
                result = _stats.accuracy / _stats.strength + _stats.endurance + 0.1f * _stats.speed;
                break;
        }

        return result * _stats.teaming;
    }

    public bool IsStatsZero() => _stats.strength == 0 & _stats.speed == 0 & _stats.endurance == 0 & _stats.accuracy == 0 & _stats.teaming == 0;

}

[Serializable]
public struct FootballerStats
{
    public int strength;
    public int speed;
    public int endurance;
    public int accuracy;
    public int teaming;
}

