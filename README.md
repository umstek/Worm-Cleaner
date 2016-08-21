# Worm-Cleaner
Remove known worms. Reverse the effects.

*This is not an antivirus software.*

This tool is useful to reverse the adverse effects caused by a known malware. 
What is meant by 'known' is that it should be possible to remove the worm without the help of any specific software. 
So, this is basically an automated process of worm removal which can be done manually.

## Example scenario
* `New Folder.exe` is a worm (It can copy itself, but doesn't need any othe file to hide itself in).
* It spreads via removable drives.
* It hides every and each folder in removable drive and creates a copy of the worm with the same name as the folder.
* `New Folder.exe` has the same icon as a folder; so if the user's computer has `View extensions for known file types` unchecked, unsuspecting user will open the worm in disguise. 
* When/if the user runs the worm, it will copy to user's computer and register to run whenever the computer starts. 
* While the worm is running, if user connects another uninfected removable drive, the worm copies itself to the drive. 

For the above circumstances, this tool can recover the damages caused by the worm. 

## Usage
1. If the worm is currently running, find it in task manager (Right Click `Task Bar` --> Click `Task Manager` --> Select `Process` tab --> Find the suspicious process).
2. Check `Close and remove if already running`: 
*If the worm is already running, program will force them to quit, and then delete them, even if not in a removable drive.*
3. Check `Delete worm from removable drives`: 
*Scan all removable drives and delete the worm from them.*
4. Check `Delete all shortcuts (*.lnk) from removable drives`: 
*Delete all .lnk files (shortcuts) possibly created by the worm. Check this if your contents have become shortcuts.*
5. Check `Permanently show system/hidden files`:
*Reset attributes of all the file to -s -h. If you can't see the files you put into the drive, check this.*
6. Finally click `Clean` to perform selected actions. 

## License
// TBD
