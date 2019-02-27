using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2D {
    public static double Scale(int input)
    {
        var v = new Vector2();
        var vScaled = v * input;
        return vScaled.x;
    }

    public static double Multiply(int input)
    {
        var v = new Vector2();
        var v2 = new Vector2(input, input);
        var vMult = new Vector3(v.x * v2.x, v.y * v2.y);
        return vMult.x;
    }

    public static double Translate(int input)
    {
        var v = new Vector2();
        var v2 = new Vector2(input, input);
        var vTranslated = v + v2;
        return vTranslated.x;
    }

    public static double Subtract(int input)
    {
        var v = new Vector2();
        var v2 = new Vector2(input, input);
        var vSub = v2 - v;
        return vSub.x;
    }

    public static double Length(int input)
    {
        var v = new Vector2(input, input);
        return v.magnitude;
    }

    public static double Dot(int input)
    {
        var v = new Vector2();
        var v2 = new Vector2(input, input);
        var dot = Vector2.Dot(v, v2);
        return dot;
    }

}
