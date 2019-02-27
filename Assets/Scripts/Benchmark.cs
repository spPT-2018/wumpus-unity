using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Benchmark : MonoBehaviour {
    private static StreamWriter LogFile;
    private static string mode;

    public static void OpenLogFile(string filename)
    {
        var curDir = Directory.GetCurrentDirectory();
        Debug.Log(curDir);
        string fullPath;
        if(Application.isEditor)
            fullPath = $"{curDir}/../results/{filename}";
        else
            fullPath = $"{curDir}/../../results/{filename}";

        if (File.Exists(fullPath))
        {
            Debug.Log("Deleting old result file");
            File.Delete(fullPath);
        }

        LogFile = new StreamWriter(fullPath);

        LogFile.WriteLine("Test,Mean,Deviation,Count");
    }

    public static void CloseLogFile()
    {
        LogFile.Flush();
        LogFile.Close();
    }

    void Start()
    {
        if (Application.isEditor)
            mode = "editor";
        else
            mode = "release";
        OpenLogFile($"Unity C# ({mode}).csv");
    }

    private void OnDestroy()
    {
        CloseLogFile();
    }


    public static double Mark8(string msg, Func<int, double> fun,
            int iterations, double minTime)
    {
        int count = 1, totalCount = 0;
        double dummy = 0.0, runningTime = 0.0, deltaTime = 0.0, deltaTimeSquared = 0.0;
        do
        {
            count *= 2;
            deltaTime = 0.0;
            deltaTimeSquared = 0.0;
            for (int j = 0; j < iterations; j++)
            {
                Timer t = new Timer();
                for (int i = 0; i < count; i++)
                {
                    dummy += fun(i);
                }
                runningTime = t.Check();
                double time = runningTime / count;
                deltaTime += time;
                deltaTimeSquared += time * time;
                totalCount += count;
            }
        } while (runningTime < minTime && count < Int32.MaxValue / 2);

        double mean = deltaTime / iterations,
            standardDeviation = Math.Sqrt((deltaTimeSquared - mean * mean * iterations) / (iterations - 1));
        LogFile.WriteLine($"{msg},{mean},{standardDeviation},{count}");
        return dummy / totalCount;
    }

    bool testRun = false;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !testRun)
        {
            testRun = true;

            Benchmark.Mark8("ScaleVector2D", Test2D.Scale, 5, MsToNs(250));
            Benchmark.Mark8("ScaleVector3D", Test3D.Scale, 5, MsToNs(250));
            Benchmark.Mark8("MultiplyVector2D", Test2D.Multiply, 5, MsToNs(250));
            Benchmark.Mark8("MultiplyVector3D", Test3D.Multiply, 5, MsToNs(250));
            Benchmark.Mark8("TranslateVector2D", Test2D.Translate, 5, MsToNs(250));
            Benchmark.Mark8("TranslateVector3D", Test3D.Translate, 5, MsToNs(250));
            Benchmark.Mark8("SubstractVector2D", Test2D.Subtract, 5, MsToNs(250));
            Benchmark.Mark8("SubstractVector3D", Test3D.Subtract, 5, MsToNs(250));
            Benchmark.Mark8("LengthVector2D", Test2D.Length, 5, MsToNs(250));
            Benchmark.Mark8("LengthVector3D", Test3D.Length, 5, MsToNs(250));
            Benchmark.Mark8("Dotproduct2D", Test2D.Dot, 5, MsToNs(250));
            Benchmark.Mark8("Dotproduct3D", Test3D.Dot, 5, MsToNs(250));
            Benchmark.Mark8("MemTest", TestMath.MemTest, 5, MsToNs(250));
            Benchmark.Mark8("Prime", TestMath.Primes, 5, MsToNs(250));
            Benchmark.Mark8("Sestoft", TestMath.Sestoft, 5, MsToNs(250));
            Benchmark.Mark8("SestoftPow", TestMath.SestoftPow, 5, MsToNs(250));

            Debug.Log("Tests done");
        }
    }

    static double MsToNs(long ms) => ms * 1000000D;
}
