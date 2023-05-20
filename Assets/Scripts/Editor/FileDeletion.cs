using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FileDeletion : Editor
{
    [MenuItem("Custom/Delete File")]
    static void DeleteFile() {
        string filePath = Application.persistentDataPath + DataManager.FileName;
        if (File.Exists(filePath)) {
            File.Delete(filePath);
            Debug.Log("File delete.");
        } else {
            Debug.Log("File is not exist.");
        }
    }
}
