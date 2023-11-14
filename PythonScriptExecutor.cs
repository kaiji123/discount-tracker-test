using System;
using System.Diagnostics;

public class PythonScriptExecutor
{
    public static void ExecutePythonScript(string scriptPath)
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "python.exe"; // Replace with the path to your Python interpreter
            process.StartInfo.Arguments = scriptPath; // Path to the Python script to execute
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            // Read the output from the Python script (if needed)
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);

            process.WaitForExit();
        }
    }
}
