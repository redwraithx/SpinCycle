
using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetworkProfile;


namespace Data
{
    public class DataClass : MonoBehaviour
    {
        public static void SaveProfile(ProfileData _profile)
        {
            try
            {
                // sdt = saved data
                string path = Application.persistentDataPath + "/userData.sdt";
                
                Debug.Log("save path: " + path);

                if (File.Exists(path + path))
                {
                    Debug.Log("Path exists, deleting");
                    File.Delete(path);
                }

                FileStream file = File.Create(path);

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file, _profile);
                
                file.Close();
                
                Debug.Log("Profile saved successfully");
                
            }
            catch (Exception e)
            {
                Debug.Log($"Error something went wrong creating profile: {e.Message}");
            }
        }

        public static ProfileData LoadProfile()
        {
            ProfileData profile = new ProfileData();
            
            try
            {
                string path = Application.persistentDataPath + "/userData.sdt";

                if (File.Exists(path))
                {
                    FileStream file = File.Open(path, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();

                    profile = (ProfileData) bf.Deserialize(file);
                    
                    Debug.Log("profile Loaded from saved file, successfully");
                }
            }
            catch(Exception e)
            {
                Debug.Log($"File was not found at: {e.Message}");

                return null;
            }
            
            return profile;
        }
        
    }

}