using System;
using System.Collections.Generic;
using System.Threading;

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

public class IdGenerator
{
    private static object instance = new object();

    public static string NewId()
    {
        lock (instance)
        {
            Thread.Sleep(1);
            long ticks = DateTime.Now.Ticks;
            return ConvertToBase(ticks, 10);
        }
    }

    private static string ConvertToBase(long num, int nbase)
    {
        string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        // check if we can convert to another base
        if (nbase < 2 || nbase > chars.Length)
            return "";

        long r;
        string newNumber = "";

        // in r we have the offset of the char that was converted to the new base
        while (num >= nbase)
        {
            r = num % nbase;
            newNumber = chars[(int)r] + newNumber;
            num = num / nbase;
        }
        // the last number to convert
        newNumber = chars[(int)num] + newNumber;

        return newNumber.ToLower();
    }
}

[Serializable]
public struct StructDictionary<K,V>
{
    public K Key;
    public V Value;
}

