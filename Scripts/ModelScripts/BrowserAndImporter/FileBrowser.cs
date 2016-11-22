using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/* This script contains functions to process the user's local file system
 * It importantly contains two lists:
 	* A list containing all file and subdirectory content in the current directory
 	* A list containing the file and subdirectory NAMES within the current directory
 */
public class FileBrowser : MonoBehaviour {

	//The starting folder for the filebrowser
	public string initialFolderPath = "C:/";
	//The import manager that will handle files given by this File Browser
	public aiImportManager importManager;
	//The folder currently being accessed by the filebrowser
	public DirectoryInfo currentDirectory { get; private set; }
	//A list of the files and folders contained in the current folder
	private List<FileSystemInfo> folderContents;
	//A list of the names of the files and folders in the current folder
	public List<string> folderContentNames { get; private set; }


	// Use this for initialization
	void Start () {
		currentDirectory = new DirectoryInfo(initialFolderPath);
		UpdateFolderContentLists(currentDirectory);
	}


	// Updates folder content lists based on the current directory
	private void UpdateFolderContentLists(DirectoryInfo currentDir) {
		folderContents = GetFolderContents(currentDir);
		folderContentNames = GetFolderContentNames(folderContents);
	}


	// Returns a list of all files and folders within the current directory
	// If we want to restrict the filetypes appearing in the browser, this will be the function to edit.
	public List<FileSystemInfo> GetFolderContents(DirectoryInfo currentDir) 
	{
		List<FileSystemInfo> contents = new List<FileSystemInfo>();

		//Add the parent directory as the first item in the list
		if(currentDir.Parent != null)
		{
			contents.Add(currentDir.Parent);
		}
		//Add all subfolders to the list
		contents.AddRange(currentDir.GetDirectories());	
		//Add all obj files in the current directory to the list	
		contents.AddRange(currentDir.GetFiles("*obj"));
		//Add all fbx files in the current directory to the list	
		contents.AddRange(currentDir.GetFiles("*fbx"));
		//Add all dae files in the current directory to the list	
		contents.AddRange(currentDir.GetFiles("*dae"));
		//Add all 3ds files in the current directory to the list	
		contents.AddRange(currentDir.GetFiles("*3ds"));
		//Add all ms3d files in the current directory to the list	
		contents.AddRange(currentDir.GetFiles("*ms3d"));

		return contents;
	}


	// Given a list of files and folders, returns a list of the names
	public List<string> GetFolderContentNames(List<FileSystemInfo> folderContents) 
	{
		List<string> names = new List<string>();

		foreach(FileSystemInfo content in folderContents)
		{
			names.Add(content.Name);
		}

		return names;
	}


	// Called on button press
	// Checks the selected text (file sytem item name) and performs operations depending on whether it is a folder or file
	public void HandleSelectedText(Text selectedText) 
	{
		string selectedName = selectedText.text;

		// If selectedName is valid
		if(selectedName != "" && selectedName != null)
		{
			int selectedIndex = folderContentNames.IndexOf(selectedName);

			if(selectedIndex != -1) 
			{
				FileSystemInfo selectedContent = folderContents[selectedIndex];

				//If the selected label points to a directory
				if(Directory.Exists(selectedContent.FullName))
				{
					currentDirectory = new DirectoryInfo(selectedContent.FullName);
					UpdateFolderContentLists(currentDirectory);
				}
				//If the selected label points to a file
				else if(File.Exists(selectedContent.FullName))
				{
					importManager.HandleImport(selectedContent.FullName);
				}
			}

		}
	}
}
