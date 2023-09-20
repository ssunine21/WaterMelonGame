using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class User {
	public int coin;
	public int lastjoin;
	public int exp;

	public User(int coin, int lastjoin, int exp) {
		this.coin = coin;
		this.lastjoin = lastjoin;
		this.exp = exp;
	}
}