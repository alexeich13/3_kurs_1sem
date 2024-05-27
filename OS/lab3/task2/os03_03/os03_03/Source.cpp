#include <iostream>
#include <windows.h>
#include <tlhelp32.h>
#include <string>

int main() {
    system("C:\\Windows\\System32\\tasklist.exe");
    /*HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (hSnapshot == INVALID_HANDLE_VALUE) {
        std::cerr << "Error creating process snapshot." << std::endl;
        return 1;
    }

    PROCESSENTRY32 pe32;
    pe32.dwSize = sizeof(PROCESSENTRY32);

    if (!Process32First(hSnapshot, &pe32)) {
        std::cerr << "Error getting information about the first process." << std::endl;
        CloseHandle(hSnapshot);
        return 1;
    }

    std::cout << "List of running processes:" << std::endl;
    do {
        TCHAR szProcessPath[MAX_PATH];
        HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, pe32.th32ProcessID);
        if (hProcess) {
            DWORD pathSize = sizeof(szProcessPath) / sizeof(TCHAR);
            if (QueryFullProcessImageName(hProcess, 0, szProcessPath, &pathSize)) {
                std::wcout << L"Process Name: " << szProcessPath << std::endl;
            }
            CloseHandle(hProcess);
        }
        std::wcout << L"Process ID (PID): " << pe32.th32ProcessID << std::endl;
        std::cout << "-----------------------------------" << std::endl;
    } while (Process32Next(hSnapshot, &pe32));

    CloseHandle(hSnapshot);

    return 0;*/
}
