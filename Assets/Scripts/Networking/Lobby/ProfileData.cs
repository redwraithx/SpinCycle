

namespace NetworkProfile
{
    [System.Serializable]
    public class ProfileData
    {
        public string userName;
        
        public bool hasPlayedTutorial;

        public float musicVolume;
        public float sfxVolume;
        
        //public ulong scoreOverPlayersLife;

        public ProfileData()
        {
            userName = "";
            hasPlayedTutorial = false;
            musicVolume = 0.8f;
            sfxVolume = 0.8f;
            
            //scoreOverPlayersLife = 0;
        }

        public ProfileData(string userName, bool hasPlayedTutorial, float musicVol, float sfxVol)//, ulong lifeTimeScore)
        {
            this.userName = userName;
            this.hasPlayedTutorial = hasPlayedTutorial;
            musicVolume = musicVol;
            sfxVolume = sfxVol;


            // this.scoreOverPlayersLife = lifeTimeScore;
        }

    }
}
