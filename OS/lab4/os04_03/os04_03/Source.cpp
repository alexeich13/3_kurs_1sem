#include <iostream>
#include <thread>
#include <windows.h>

DWORD WINAPI OS04_03_T1() {
    for (int i = 1; i <= 50; i++) {
        DWORD processId = GetCurrentProcessId();
        DWORD threadId = GetCurrentThreadId();
        std::cout << i << ". Поток T1 - Идентификатор процесса: " << processId << ", Идентификатор потока: " << threadId << std::endl;
        Sleep(100);
    }
    return 0;
}

DWORD WINAPI OS04_03_T2() {
    for (int i = 1; i <= 125; i++) {
        DWORD processId = GetCurrentProcessId();
        DWORD threadId = GetCurrentThreadId();
        std::cout << i << ". Поток T2 - Идентификатор процесса: " << processId << ", Идентификатор потока: " << threadId << std::endl;
        Sleep(100);
    }
    return 0;
}


int main() {

    setlocale(LC_CTYPE, "rus");
    DWORD processId = GetCurrentProcessId();
    DWORD parentId = GetCurrentThreadId();
    DWORD childId_T1 = NULL;
    DWORD childId_T2 = NULL;
    HANDLE handleClild_T1 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)OS04_03_T1, NULL, 0, &childId_T1);
    HANDLE handleClild_T2 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)OS04_03_T2, NULL, 0, &childId_T2);
    for (int i = 1; i < 100; i++) {
        std::cout << i << ". Основной процесс - Идентификатор процесса: " << processId << std::endl;
        Sleep(100);
        if (i == 20) {
            SuspendThread(handleClild_T1);
        }
        if (i == 60) {
            ResumeThread(handleClild_T1);
        }
        if (i == 40) {
            SuspendThread(handleClild_T2);
        }
        if (i == 95) {
            ResumeThread(handleClild_T2);
        }
    }
    WaitForSingleObject(handleClild_T1, INFINITE);
    WaitForSingleObject(handleClild_T2, INFINITE);
    CloseHandle(handleClild_T1);
    CloseHandle(handleClild_T2);
    system("pause");

    return 0;
}