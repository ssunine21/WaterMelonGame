using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class User {
	public int score;
	public int coin;
	public int lastjoin;

	public User(int bestScore, int coin, int lastjoin) {
		this.score = bestScore;
		this.coin = coin;
		this.lastjoin = lastjoin;
	}
}