//---------------------------------------------------------------------------------------------------------------------
// This class is based on the original code of the DirectoryInfo class in the Mono project. It has been created as
// an "Editor Script" because some functions used in this class aren't available at runtime in Webplayer builds.
//---------------------------------------------------------------------------------------------------------------------
// 
// System.IO.DirectoryInfo.cs 
//
// Authors:
//   Miguel de Icaza, miguel@ximian.com
//   Jim Richardson, develop@wtfo-guru.com
//   Dan Lewis, dihlewis@yahoo.co.uk
//   Sebastien Pouliot  <sebastien@ximian.com>
//   Marek Safar  <marek.safar@gmail.com>
//
// Copyright (C) 2002 Ximian, Inc.
// Copyright (C) 2001 Moonlight Enterprises, All Rights Reserved
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
// Copyright (C) 2014 Xamarin, Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System.IO;
using System;
using System.Collections.Generic;
using UnityEditor;

public static class DirectoryInfoEditorExtensions{
	#region Extension Methods
	public static IEnumerable<DirectoryInfo> EnumerateDirectories(this DirectoryInfo directoryInfo){
		return directoryInfo.EnumerateDirectories ("*", SearchOption.TopDirectoryOnly);
	}
	
	public static IEnumerable<DirectoryInfo> EnumerateDirectories(
		this DirectoryInfo directoryInfo, 
		string searchPattern
	){
		return directoryInfo.EnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
	}
	
	public static IEnumerable<DirectoryInfo> EnumerateDirectories(
		this DirectoryInfo directoryInfo,
		string searchPattern, 
		SearchOption searchOption
	){
		if (searchPattern == null){
			throw new ArgumentNullException ("searchPattern");
		}
		
		return DirectoryInfoEditorExtensions.CreateEnumerateDirectoriesIterator (directoryInfo, searchPattern, searchOption);
	}
	
	public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo directoryInfo){
		return directoryInfo.EnumerateFiles ("*", SearchOption.TopDirectoryOnly);
	}
	
	public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo directoryInfo, string searchPattern){
		return directoryInfo.EnumerateFiles (searchPattern, SearchOption.TopDirectoryOnly);
	}
	
	public static IEnumerable<FileInfo> EnumerateFiles(
		this DirectoryInfo directoryInfo, 
		string searchPattern, 
		SearchOption searchOption
	){
		if (searchPattern == null){
			throw new ArgumentNullException ("searchPattern");
		}
		
		return DirectoryInfoEditorExtensions.CreateEnumerateFilesIterator(directoryInfo, searchPattern, searchOption);
	}

	public static IEnumerable<FileSystemInfo> EnumerateFileSystemInfos (this DirectoryInfo directoryInfo){
		return directoryInfo.EnumerateFileSystemInfos ("*", SearchOption.TopDirectoryOnly);
	}
	
	public static IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(
		this DirectoryInfo directoryInfo, 
		string searchPattern
	){
		return directoryInfo.EnumerateFileSystemInfos (searchPattern, SearchOption.TopDirectoryOnly);
	}
	
	public static IEnumerable<FileSystemInfo> EnumerateFileSystemInfos (
		this DirectoryInfo directoryInfo, 
		string searchPattern, 
		SearchOption searchOption
	){
		if (searchPattern == null)
			throw new ArgumentNullException ("searchPattern");
		if (searchOption != SearchOption.TopDirectoryOnly && searchOption != SearchOption.AllDirectories)
			throw new ArgumentOutOfRangeException ("searchoption");
		
		return DirectoryInfoEditorExtensions.CreateEnumerateFileSystemInfosIterator(directoryInfo, searchPattern, searchOption);
	}
	#endregion

	#region private class methods
	private static IEnumerable<FileInfo> CreateEnumerateFilesIterator(
		this DirectoryInfo directoryInfo,
		string searchPattern, 
		SearchOption searchOption
	){
		foreach (FileInfo file in directoryInfo.GetFiles(searchPattern, searchOption)){
			yield return file;
		}
	}

	private static IEnumerable<DirectoryInfo> CreateEnumerateDirectoriesIterator(
		DirectoryInfo directoryInfo,
		string searchPattern, 
		SearchOption searchOption
	){
		foreach (DirectoryInfo dir in directoryInfo.GetDirectories(searchPattern, searchOption)){
			yield return dir;
		}
	}

	private static IEnumerable<FileSystemInfo> CreateEnumerateFileSystemInfosIterator(
		DirectoryInfo directoryInfo,
		string searchPattern, 
		SearchOption searchOption
	){
		// TODO: use searchOption
		foreach (FileSystemInfo fileSystem in directoryInfo.GetFileSystemInfos(searchPattern)){
			yield return fileSystem;
		}
	}
	#endregion

}
