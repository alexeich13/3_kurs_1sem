#include <iostream>
#include <windows.h>

int main() {
    DWORD processId = GetCurrentProcessId();

    STARTUPINFO si1 = {};
    PROCESS_INFORMATION pi1 = {};
    if (CreateProcess(
        L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab3\\task2\\os03_02_01\\x64\\Debug\\os03_02_01.exe", 
        NULL,
        NULL,
        NULL,
        FALSE,
        0,
        NULL,
        NULL,
        &si1,
        &pi1)) {
        CloseHandle(pi1.hProcess);
        CloseHandle(pi1.hThread);
    }
    else {
        std::cerr << "Fault on OS03_02_01." << std::endl;
    }

    STARTUPINFO si2 = {};
    PROCESS_INFORMATION pi2 = {};
    if (CreateProcess(
        L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab3\\task2\\os03_02_02\\x64\\Debug\\os03_02_02.exe", 
        NULL,
        NULL,
        NULL,
        FALSE,
        0,
        NULL,
        NULL,
        &si2,
        &pi2)) {
        CloseHandle(pi2.hProcess);
        CloseHandle(pi2.hThread);
    }
    else {
        std::cerr << "Fault on OS03_02_2." << std::endl;
    }

    for (int i = 0; i < 100; i++) {
        std::cout << "Identifier of OS03_02: " << processId << " Iteration: " << i + 1 << std::endl;
        Sleep(1000); 
    }

    return 0;
}