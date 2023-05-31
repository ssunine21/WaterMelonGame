using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Firebase;
using Firebase.Database;
using Firebase.Editor;

public class FileDeletion : Editor
{
    [MenuItem("Custom/Delete File")]
    static void DeleteFile()
    {
        string filePath = Application.persistentDataPath + DataManager.FileName;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("File delete.");
        }
        else
        {
            Debug.Log("File is not exist.");
        }
    }

    [MenuItem("Custom/Delete Firebase UserData")]
    static void DeleteFirebase()
    {
        DatabaseReference database;

        // Firebase 초기화GetReference(TITLE)
        //FirebaseDatabase.DefaultInstance.GetReferenceFromUrl("https://mergegame-e68c3-default-rtdb.firebaseio.com");

        // 데이터베이스 레퍼런스 설정
        //database = FirebaseDatabase.DefaultInstance.GetReference("RANK").get;

        try {
            FirebaseDatabase.DefaultInstance
                .GetReference("RANK").OrderByChild("score").EqualTo(0)
                .GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                    } else if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;

                        foreach (var item in snapshot.Children) {
                            FirebaseDatabase.DefaultInstance.GetReference("RANK").Child(item.Key).RemoveValueAsync();
                            Debug.Log(item.Key);
                        }
                    }
                });
            Debug.Log("조건에 맞는 데이터가 성공적으로 삭제되었습니다.");
        } catch (System.Exception e) {
            Debug.LogError("데이터 삭제 중 오류가 발생했습니다: " + e.Message);
        }
    }
}
