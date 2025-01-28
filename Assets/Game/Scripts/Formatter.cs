using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Formatter
{
    static readonly string[] suffixes = { "", "K", "M", "G" };
    public static string Format(long cash, string prefix = "")
    {
        int k;
        if (cash == 0)
            k = 0;    // log10 of 0 is not valid
        else
            k = (int)(Mathf.Log10(cash) / 3); // get number of digits and divide by 3
        var dividor = Mathf.Pow(10, k * 3);  // actual number we print
        var text = k != 0 ? prefix + (cash / dividor).ToString("F1") + suffixes[k] : prefix + (cash / dividor).ToString("#0") + suffixes[k];
        return text;
    }
    public static string Format(float cash, string prefix = "")
    {
        int k;
        if (cash == 0)
            k = 0; // log10 of 0 is not valid
        else
            k = (int)(Mathf.Log10(Mathf.Abs(cash)) / 3); // get number of digits and divide by 3, ensuring cash is positive for log calculation
        var divisor = Mathf.Pow(10, k * 3); // actual number we print

        try
        {
            // Adding safety check for suffixes array bounds
            string suffix = k < suffixes.Length ? suffixes[k] : "???";
            
            var text = k != 0 ? prefix + (cash / divisor).ToString("F1") + suffix : prefix + (cash / divisor).ToString("#0") + suffix;
            return text;
        }
        catch (System.Exception ex)
        {
            Debug.Log($"An error occurred: {ex.Message}");
        }
        return "0";
    }
}
