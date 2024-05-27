@echo off 
chcp 65001  > nul 2>&1
echo -- строка параметров: %*
echo -- параметр 1: %1
echo -- параметр 2: %2
echo -- параметр 3: %3
set /a sum1=%1 + %2
set /a sum2=%1 * %2
set /a sum3=%3 / %2
set /a sum4=%2 - %1
set /a sum5 = (%2 - %1)*(%1 - %2)
echo -- %1 + %2 = %sum1%
echo -- %1 * %2 = %sum2%
echo -- %3 / %2 = %sum3%
echo -- %2 - %1 = %sum4%
echo -- (%2 - %1)*(%1 - %2) = %sum5%