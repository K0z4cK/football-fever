using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType {Strength, Speed, Endurance, Accuracy, Teaming};

public class Footballer
{
    private string _name;
    public string Name => _name;

    private FootballerStats _stats;
    public FootballerStats Stats => _stats;

    private int _points;

    private float _rating;
    public float Rating => _rating; 

    public Footballer(string name)
    {
        _name = name;

        _stats = new FootballerStats()
        {
            strength = 1,
            speed = 1,
            endurance = 1,
            accuracy = 1,
            teaming = 1
        };
        _points = 7;
        _rating = 1;
    }

    public void SetStatsRandom()
    {
        _points = 0;
        int randomPoints = 10;
        int nextPoints = Random.Range(0, randomPoints + 1);
        randomPoints -= nextPoints;
        _stats.strength += nextPoints;

        nextPoints = Random.Range(0, randomPoints + 1);
        randomPoints -= nextPoints;
        _stats.speed += nextPoints;

        nextPoints = Random.Range(0, randomPoints + 1);
        randomPoints -= nextPoints;
        _stats.endurance += nextPoints;

        nextPoints = Random.Range(0, randomPoints + 1);
        randomPoints -= nextPoints;
        _stats.accuracy += nextPoints;

        nextPoints = Random.Range(0, randomPoints + 1);
        randomPoints -= nextPoints;
        _stats.teaming += nextPoints;

        _rating = GetStatsResult();
    }

    public void IncreaseStat(StatType type)
    {
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

