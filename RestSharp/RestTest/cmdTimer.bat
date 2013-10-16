@echo off
:loop
call run
echo Waiting For Ten Minutes... 
TIMEOUT /T 60 /NOBREAK
goto loop