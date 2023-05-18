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
            Debug.Log("파일이 삭제되었습니다.");
        } else {
            Debug.Log("파일이 존재하지 않습니다.");
        }
    }
}
