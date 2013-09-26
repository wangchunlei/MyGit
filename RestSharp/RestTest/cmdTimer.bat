@echo off
:loop
call run
echo Waiting For One Hour... 
TIMEOUT /T 60 /NOBREAK
goto loop