

namespace NetworkProfile
{
    [System.Serializable]
    public class ProfileData
    {
        public string userName;

        public bool hasPlayedTutorial;
        //public ulong scoreOverPlayersLife;

        public ProfileData()
        {
            userName = "";
            hasPlayedTutorial = false;
            //scoreOverPlayersLife = 0;
        }

        public ProfileData(string userName, bool hasPlayedTutorial)//, ulong lifeTimeScore)
        {
            this.userName = userName;
            this.hasPlayedTutorial = hasPlayedTutorial;
            // this.scoreOverPlayersLife = lifeTimeScore;
        }

    }
}
