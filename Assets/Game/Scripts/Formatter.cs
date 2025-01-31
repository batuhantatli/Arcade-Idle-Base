using UnityEngine;

public static class Formatter
{
    static readonly string[] suffixes = { "", "K", "M", "G" };
    public static string Format(float cash, string prefix = "")
    {
        int k;
        if (cash == 0)
            k = 0; // log10 of 0 is not valid
        else
            k = (int)(Mathf.Log10(Mathf.Abs(cash)) / 3); 
        var divisor = Mathf.Pow(10, k * 3); 

        try
        {
         
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
