
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
	private string fileName = @"Logins.txt";
	public Button LoginPress;
	public InputField userName;
	public InputField passWord;

	private string hashUser;
	private string hashPass;
	private void Start()
	{

		LoginPress.onClick.AddListener(Log);
	}
	private void Log()
    {
		hashUser = Hasher.GetHashString(userName.text);
		hashPass = Hasher.GetHashString(passWord.text);
		if (IsInFile())
        {
			SceneManager.LoadScene("110_GuardianModeMenu");
        }
    }
	private Boolean IsInFile()
    {
		string line = "";
		StreamReader fileReader = new StreamReader(@"C:\Users\izak\CSC131_Project-Autism-Target-Learning\01_FrontEnd\Project_AutismTargetLearning\Assets\Scripts\LoginScripts\Logins.txt");
		while ((line = fileReader.ReadLine()) != null)
		{
			if (line.Contains(hashUser+hashPass))
			{
				fileReader.Close();
				return true;
			}
		}
		fileReader.Close();
		return false;
    }
}
