public class User {
	public int coin;
	public int lastjoin;
	public int exp;
	public int bestScore;
	public bool isRemoveAds;

	public User(int coin, int lastjoin, int exp, int bestScore, bool isRemoveAds) {
		this.coin = coin;
		this.lastjoin = lastjoin;
		this.exp = exp;
		this.bestScore = bestScore;
		this.isRemoveAds = isRemoveAds;
	}
}