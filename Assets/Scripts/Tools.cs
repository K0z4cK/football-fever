using System;
using System.Collections.Generic;

public static class RandomTools 
{
    private static Random rng = new Random();
    public static Random Random => rng;

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

[Serializable]
public struct StructDictionary<K,V>
{
    public K Key;
    public V Value;
}

