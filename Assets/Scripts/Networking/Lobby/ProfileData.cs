namespace NetworkProfile
{
    [System.Serializable]
    public class ProfileData
    {
        public string userName;

        public bool hasPlayedTutorial;

        public float musicVolume;
        public float sfxVolume;

        public int highestScore;
        public int gamesWon;
        public int gamesLost;
        public int gamesDraws;
        public int gamesIncomplete;

        // index for the player model you have set as well

        // index of each custom object the player may have like a hat, 1 public int for each.

        //public ulong scoreOverPlayersLife;

        public ProfileData()
        {
            userName = "";
            hasPlayedTutorial = false;
            musicVolume = 0.8f;
            sfxVolume = 0.8f;

            highestScore = 0;
            gamesWon = 0;
            gamesLost = 0;
            gamesDraws = 0;
            gamesIncomplete = 0;

            //scoreOverPlayersLife = 0;
        }

        public ProfileData(string userName, bool hasPlayedTutorial, float musicVol, float sfxVol, int highScore = 0, int won = 0, int lost = 0, int draws = 0, int incomplete = 0)//, ulong lifeTimeScore)
        {
            this.userName = userName;
            this.hasPlayedTutorial = hasPlayedTutorial;
            musicVolume = musicVol;
            sfxVolume = sfxVol;

            highestScore = highScore;
            gamesWon = won;
            gamesLost = lost;
            gamesDraws = draws;
            gamesIncomplete = incomplete;

            // this.scoreOverPlayersLife = lifeTimeScore;
        }
    }
}