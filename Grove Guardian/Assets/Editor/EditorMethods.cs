using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EdiorMethods : Editor {
    const string extension = ".cs";
    public static void WriteToEnum<T>(string path, string name, ICollection<T> data) {
        using (StreamWriter file = File.CreateText(path + name + extension)) {
            file.WriteLine("public enum " + name + " {");
            int i = 0;
            foreach (var line in data) {
                string lineRep = line.ToString().Replace(" ", string.Empty);
                if (!string.IsNullOrEmpty(lineRep)) {
                    file.WriteLine(string.Format("	{0} = {1},",
                        lineRep, i));
                    i++;
                }
            }
            file.WriteLine("}");
        }
        AssetDatabase.ImportAsset(path + name + extension);
    }
}
