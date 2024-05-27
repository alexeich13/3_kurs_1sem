@echo off 
chcp 65001  > nul 2>&1
echo --текущий пользователь: %USERNAME%
echo --текущие дата и время: %DATE% %TIME%
echo --имя компьютера: %COMPUTERNAME%