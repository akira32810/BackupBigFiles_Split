# BackupBigFiles_Split
splitting big files to backup to the cloud and combine them after

When the app run, it will create a config file with a template and tell you where it is.  The number in the text file on the third line (in megabytes) represents how maximum file size for each file pieces.
Assumming you have a cloud service setup as a location in your machine, just put the location on the 4th line of the config file.

Logs will be create at the end of each run and append to the same log file.

<hr/>
Prerequisite: dotnetcore 2.1+ </br>
Compatible with Windows, Linux and OSx
