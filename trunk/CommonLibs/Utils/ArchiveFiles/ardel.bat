@echo off
set SourceFolder=%1
set ArchNameSufix=%2
set RemoteFolder=%3
set Delete=%4

dir /b /a "%SourceFolder%*" | >nul findstr "^" && (
	REM Files and/or Folders exist
	goto cont
) || (
	REM No File or Folder found
	goto exit
)

:cont
set Dtmp=%Date%%Time%
set DD=%Dtmp:~0,2%
set DM=%Dtmp:~3,2%
set DY=%Dtmp:~6,4%
set TH=%Dtmp:~10,2%
set TM=%Dtmp:~13,2%
set TS=%Dtmp:~16,2%
set TSS=%Dtmp:~19,2%

set DT=%DY%-%DM%-%DD%_%TH%%TM%%TS%%TSS%
echo %DT%

7zip\7z.exe a "%TMP%\%ArchNameSufix%_%DT%.zip" -tzip -mx=9 -mmt=on -scsWIN -ir!%SourceFolder%*

move /Y "%TMP%\%ArchNameSufix%_%DT%.zip" "%RemoteFolder%"

if /I "%Delete%" == "/Y" goto delete
goto exit

:delete
for /D %%p in ("%SourceFolder%*.*") do rmdir "%%p" /s /q
del /Q "%SourceFolder%*.*"

:exit