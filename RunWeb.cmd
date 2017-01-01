@echo off

SET IISEXP=
if EXIST "c:\Program Files\IIS Express\iisexpress.exe" SET IISEXP=c:\Program Files\IIS Express\iisexpress.exe
if EXIST "c:\Program Files (x86)\IIS Express\iisexpress.exe" SET IISEXP=c:\Program Files (x86)\IIS Express\iisexpress.exe
if "%IISEXP%"=="" goto errornoiis

"%IISEXP%" /systray:true /trace:info %*
if errorlevel 1 goto error
goto success

:success
exit /b 0
goto end

:errornoiis
echo Error - Can't locate IIS Express
goto error

:error
exit /b 1
goto end

:end