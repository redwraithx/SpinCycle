

namespace NetworkProfile
{
    [System.Serializable]
    public class ProfileData
    {
        public string userName;
        //public ulong scoreOverPlayersLife;

        public ProfileData()
        {
            userName = "";
            //scoreOverPlayersLife = 0;
        }

        public ProfileData(string userName)//, ulong lifeTimeScore)
        {
            this.userName = userName;
           // this.scoreOverPlayersLife = lifeTimeScore;
        }

    }
}
