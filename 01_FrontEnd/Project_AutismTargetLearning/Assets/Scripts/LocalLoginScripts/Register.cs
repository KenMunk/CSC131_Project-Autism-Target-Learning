
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
	private string fileName = @"Logins.txt";
	public Button RegisterPress;
	public InputField userName;
	public InputField passWord;
	public InputField conPassWord;
	public InputField eMail;

	private string hashUser;
	private string hashPass;

	private void Start()
	{

		RegisterPress.onClick.AddListener(Reg);

	}
	private void Reg()
	{
		if (passWord.text.Contains(conPassWord.text))
		{
			hashUser = Hasher.GetHashString(userName.text);
			hashPass = Hasher.GetHashString(passWord.text);
			string email = eMail.text;
			if (!IsInFile())
			{
				StreamWriter fileWriter = File.AppendText(@"C:\Users\izak\CSC131_Project-Autism-Target-Learning\01_FrontEnd\Project_AutismTargetLearning\Assets\Scripts\LoginScripts\Logins.txt");
				fileWriter.WriteLine(hashUser + hashPass + " " + email);
				fileWriter.Close();
				SceneManager.LoadScene("100_LoginScreen");
			}
		}
	}
	private Boolean IsInFile()
	{
		string line = "";
		StreamReader fileReader = new StreamReader(@"C:\Users\izak\CSC131_Project-Autism-Target-Learning\01_FrontEnd\Project_AutismTargetLearning\Assets\Scripts\LoginScripts\Logins.txt");
		while ((line = fileReader.ReadLine()) != null)
		{
			if (line.Contains(hashUser + hashPass))
			{
				fileReader.Close();
				return true;
			}
		}
		fileReader.Close();
		return false;
	}
}