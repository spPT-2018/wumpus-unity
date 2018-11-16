using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMath {
    public static double Sestoft(int i)
    {
        double d = 1.1 * (double)(i & 0xFF);
        return d * d * d * d * d * d * d * d * d * d * d * d * d * d * d * d * d * d * d * d;
    }

    public static double SestoftPow(int i)
    {
        double d = 1.1 * (double)(i & 0xFF);
        return Mathf.Pow((float) d, 20);
    }

    public static double Primes(int number)
    {
        var realNumber = 100;

        var A = new bool[realNumber + 1];
        for (int i = 2; i < realNumber + 1; i++)
        {
            A[i] = true;
        }

        for (int i = 2; i < Mathf.Sqrt(realNumber); i++)
        {
            if (A[i])
            {
                var iPow = (int)Mathf.Pow(i, 2);
                var num = 0;

                for (int j = 0; j < realNumber; j = iPow + num * i)
                {
                    A[i] = false;
                    num++;
                }
            }
        }

        var primes = new List<int>();
        for (int i = 2; i < A.Length; i++)
        {
            if (A[i])
                primes.Add(i);
        }

        return primes[primes.Count - 1] & number;
    }

    public static double MemTest(int i)
    {
        var array = new double[100000];
        return array.Length & i;
    }
}
