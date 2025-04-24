// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("/X5wf0/9fnV9/X5+f8wSdVwnwIz4hT7N4SgyQFsvC5G9aSRF2iufM1RJaL5f7uBF5jxqiOgcxkanSbvF42akGb2K+M/Xk2cg2NkWcO+EG4BnYlIbtNOYogr+Ex/aiKpKUVKGzJY4k/fvCEnA77ITl+jXMk0+HkL1r4eNp0tXaNtg5paIHhsCOwB2b4zuilYZCMWO/7j17jjyYZXpAi757jYxnebGobTcfeKLAfqDJ7Z2nNCoAYDNMr3RL/N0Ga3ki+TIe+oxu1a2qEKfEEZMPn1TCHoZ8eCltlwARcMI9UaG5/oEDq/wR/SqhYm6cRZcW1GLHuAYu/O/kufhZYR82vIENiNP/X5dT3J5dlX5N/mIcn5+fnp/fO98ECbp5Chb0n18fn9+");
        private static int[] order = new int[] { 1,7,4,3,10,11,11,11,9,13,13,13,12,13,14 };
        private static int key = 127;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
