Imports System.IO
Imports System.Security.Cryptography

Public Module Core
    Private ReadOnly Property Hasher As SHA1CryptoServiceProvider = New SHA1CryptoServiceProvider

    Private Function GetFileHash(fileName As String) As String
        Dim fileStream = File.OpenRead(fileName)
        Return Convert.ToBase64String(Hasher.ComputeHash(fileStream))
    End Function

    Public Function GetTargetHashes(target As String) As HashSet(Of String)
        Dim hashes As New HashSet(Of String)

        If File.Exists(target) Then ' Input is a file
            hashes.Add(GetFileHash(target))
        Else ' Input is a process
            Dim processes = Process.GetProcessesByName(target)
            For Each process In processes
                hashes.Add(GetFileHash(process.MainModule.FileName))
            Next
        End If

        Return hashes
    End Function

    Public Sub KillByHash(fileHashes As HashSet(Of String), shouldDelete As Boolean)
        Dim processes = Process.GetProcesses()
        Dim filePath As String

        For Each process In processes
            Try
                filePath = process.MainModule.FileName
            Catch ex As Exception
                Continue For ' Skip
            End Try

            Dim fileHash = GetFileHash(filePath)

            If fileHashes.Contains(fileHash) Then
                process.Kill() ' End process

                If shouldDelete Then File.Delete(filePath) ' Delete running file
            End If
        Next
    End Sub

    Private Function GetRemovableDrives() As List(Of DriveInfo)
        Return _
            Aggregate driveInfo In DriveInfo.GetDrives() Where driveInfo.DriveType = DriveType.Removable Into ToList()
    End Function

    Public Sub CleanRemovableDrives(fileHashes As HashSet(Of String),
                                    shouldDeleteWorm As Boolean,
                                    shouldDeleteShortcuts As Boolean,
                                    shouldUnhide As Boolean)
        Dim removableDrives = GetRemovableDrives()
        Dim fileQueue = New Queue(Of FileInfo)(
            removableDrives.
                                                  Select(Function(driveInfo)
                                                      Return driveInfo.RootDirectory.EnumerateFiles(
                                                          "*",
                                                          SearchOption.AllDirectories)
                                                            End Function).
                                                  SelectMany(Function(fileInfos) fileInfos))
        For Each fileInfo In fileQueue
            Console.WriteLine(fileInfo.FullName)

            If fileInfo.Length < 10*2^20 Then ' 20 MB
                If shouldDeleteWorm AndAlso fileHashes.Contains(GetFileHash(fileInfo.FullName)) Then ' Delete worm
                    fileInfo.Delete()
                End If
            End If

            If shouldDeleteShortcuts AndAlso fileInfo.Extension = ".lnk" Then ' Delete shortcuts
                fileInfo.Delete()
            End If
            If shouldUnhide Then ' Unhide files
                fileInfo.Attributes = FileAttributes.Normal
            End If
        Next

        If shouldUnhide Then ' Unhide directories
            removableDrives.
                Select(
                    Function(driveInfo) driveInfo.RootDirectory.EnumerateDirectories("*", SearchOption.AllDirectories)).
                SelectMany(Function(infos) infos).
                ToList().ForEach(Sub(info) info.Attributes = FileAttributes.Normal)
        End If
    End Sub
End Module
