@echo off
chcp 65001  > nul 2>&1
echo -- строка параметров: %1  %2
echo -- режим: %1
echo -- имя файла: %2

if "%1"=="" (goto case1) else (goto case2)
if "%2"=="" (
    echo не задано имя файла 
    goto exit
) else if "%1" neq "создать" or if "%1" neq "удалить" (
    echo режим задан некорректно
    goto exit
)

:case1
    echo режим = {создать,удалить}
    echo файл = имя файла 
    goto exit

:case2
    if "%2"=="" (goto case3) else (goto case4)

:case3
    echo не задано имя файла 
    goto exit

:case4
if "%1" == "создать" (goto create)
if "%1" == "удалить" (goto delete) else (goto operation)
:operation
    echo режим задан некорректно
    goto exit

:create
if exist %2 (goto found)
copy NUL %2
echo файл %2 создан
goto exit

:delete
if not exist %2 (goto notfound)
del %2
echo файл %2 удален
goto exit

:found
echo файл %2 уже есть
goto exit

:notfound
echo файл %2 не найден
goto exit

:exit
