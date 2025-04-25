using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ServerTime : MonoBehaviour
{
    [SerializeField] private string url;

    public static DateTime NowTime => _serverTime;
    public static bool IsTimeReady;

    private static DateTime _serverTime;

    private void Start() {
        StartCoroutine(IeWebCheck());
    }

    public static string ToStringYesterday()
    {
        return NowTime.AddDays(-1).ToString("yyyy/MM/dd 00:00:00");
    }

    public static string ToStringNextDay()
    {
        return NowTime.AddDays(1).ToString("yyyy/MM/dd 00:00:00");
    }

    public static string ToStringParse()
    {
        return NowTime.ToString("yyyy/MM/dd hh:mm:ss");
    }

    public static int BaseTime()
    {
        int hour = DateTime.Now.Hour - DateTime.UtcNow.Hour;
        hour = hour < 0 ? hour + 24 : hour;

        return hour;
    }

    private WaitForSecondsRealtime _wfsr = new WaitForSecondsRealtime(1f);
    private IEnumerator IeWebCheck() {
        while (true) {
            IsTimeReady = false;
            float asyncTime = 0;

            UnityWebRequest request = new UnityWebRequest();
            using (request = UnityWebRequest.Get(url)) {
                yield return request.SendWebRequest();

                if (request.isNetworkError) {
                    Debug.Log("Server Time Net Error");
                } else {
                    string date = request.GetResponseHeader("date");
                    _serverTime = DateTime.Parse(date);
                }
            }

            while (asyncTime < 300) {
                IsTimeReady = true;

                yield return _wfsr;
                _serverTime.AddSeconds(1);
                asyncTime += 1;
            }
        }
    }
}
