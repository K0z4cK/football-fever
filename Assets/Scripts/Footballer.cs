using System;
using System.Linq;
using System.Collections.Generic;

public enum StatType {Strength, Speed, Endurance, Accuracy, Teaming};

public class Footballer
{
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
        //_rating = 1;
    }

    public void SetStatsRandom()
    {
        Random rnd = new Random();

        _points = 0;
        int randomPoints = 10;

        var types = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToList();
        types.Shuffle();

        foreach (var type in types)
        {
            int nextPoints = rnd.Next(randomPoints + 1);
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

}

public struct FootballerStats
{
    public int strength;
    public int speed;
    public int endurance;
    public int accuracy;
    public int teaming;
}

