using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.Review;
using Cysharp.Threading.Tasks;

public class GoogleReview : MonoBehaviour
{
    private ReviewManager _reviewManager;
	public static GoogleReview init;
	private void Awake() {
		if (init == null) {
			init = this;
		} else if (init != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);

		_reviewManager = new ReviewManager();
    }

	public async UniTask  Show() {
		var requestFlowOperation =  _reviewManager.RequestReviewFlow();
		await requestFlowOperation;

		if(requestFlowOperation.Error != ReviewErrorCode.NoError) {
			Debug.Log("Review error");
        }
		else {
			var playReviewInfo = requestFlowOperation.GetResult();
			await _reviewManager.LaunchReviewFlow(playReviewInfo);
        }
    }
}
