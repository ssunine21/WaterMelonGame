using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class User {
	public string deviceModel;
	public int coin;

	public User(string deviceModel, int coin) {
		this.deviceModel = deviceModel;
		this.coin = coin;
	}
}