@echo off 
chcp 65001  > nul 2>&1
echo -- строка параметров: %*
echo -- параметр 1: %1
echo -- параметр 2: %2
echo -- параметр 3: %3
set /a res1=%1 %3 %2
echo результат: = %res1%
