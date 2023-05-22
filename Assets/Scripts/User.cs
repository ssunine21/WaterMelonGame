using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class User {
	public int score;
	public int coin;

	public User(int bestScore, int coin) {
		this.score = bestScore;
		this.coin = coin;
	}
}