using UnityEngine;
using Cysharp.Threading.Tasks;

#if UNITY_ANDROID
using Google.Play.Review;
#endif

public class GoogleReview : MonoBehaviour
{
	public static GoogleReview init;

	private void Awake()
	{
		if (init == null)
		{
			init = this;
		}
		else if (init != this)
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);
	}

	public void ShowStoreReview()
	{
#if UNITY_ANDROID
		RequestPlayStoreReview().Forget();
#elif UNITY_IOS
		RequestAppStoreReview();
#endif
	}

	private async UniTask RequestPlayStoreReview()
	{
#if UNITY_ANDROID
		ReviewManager reviewManager = new ReviewManager();
		
		var requestFlowOperation = reviewManager.RequestReviewFlow();
		await requestFlowOperation;

		if(requestFlowOperation.Error != ReviewErrorCode.NoError) {
			Debug.Log("Review error");
        }
		else {
			var playReviewInfo = requestFlowOperation.GetResult();
			await reviewManager.LaunchReviewFlow(playReviewInfo);
        }
#endif
	}

	private void RequestAppStoreReview()
	{
#if UNITY_IOS
		if (UnityEngine.iOS.Device.systemVersion.StartsWith("10.")
		    && UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.Unknown)
		{
			UnityEngine.iOS.Device.RequestStoreReview();
		}
#endif
	}
}
