#include <iostream>
#include <Windows.h>
using namespace std;

const int TIMER_PERIOD = 6000;

int main()
{
	LPCWSTR firstPath = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab7\\x64\\Debug\\laba7.4x.exe";
	LPCWSTR secondPath = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab7\\x64\\Debug\\laba7.4x.exe";
	HANDLE hTimer = CreateWaitableTimer(NULL, FALSE, L"Global\\TMRF");
	if (hTimer == NULL)
	{
		cerr << "Error: " << GetLastError() << " - timer was not created" << endl;
		return -2;
	}
	LARGE_INTEGER dueTime;
	dueTime.QuadPart = -60000000LL;
	if (!SetWaitableTimer(hTimer, &dueTime, TIMER_PERIOD, NULL, NULL, FALSE))
	{
		cerr << "Error: " << GetLastError() << " - first timer not set" << endl;
		return -3;
	}

	STARTUPINFO si1;
	PROCESS_INFORMATION pi1;

	ZeroMemory(&si1, sizeof(si1));
	si1.cb = sizeof(&si1);
	ZeroMemory(&pi1, sizeof(pi1));

	STARTUPINFO si2;
	PROCESS_INFORMATION pi2;

	ZeroMemory(&si2, sizeof(si2));
	si2.cb = sizeof(&si2);
	ZeroMemory(&pi2, sizeof(pi2));

	if (!CreateProcess(firstPath, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si1, &pi1))
	{
		cerr << "Error: " << GetLastError() << " - first process was not started" << endl;
		return -1;
	}
	if (!CreateProcess(secondPath, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si2, &pi2))
	{
		cerr << "Error: " << GetLastError() << " - second process was not started" << endl;
		return -1;
	}
	if (WaitForSingleObject(pi1.hProcess, INFINITE) != WAIT_OBJECT_0)
	{
		cerr << "Error: " << GetLastError() << " - wait end first process failed" << endl;
	}
	else
	{
		cout << "First process exit successful" << endl;
	}
	if(WaitForSingleObject(pi2.hProcess, INFINITE) != WAIT_OBJECT_0)
	{
		cerr << "Error: " << GetLastError() << " - wait end second process failed" << endl;
	}
	else
	{
		cout << "Second process exit successful" << endl;
	}
	CloseHandle(hTimer);
	CloseHandle(pi1.hProcess);
	CloseHandle(pi2.hProcess);
	CloseHandle(pi1.hThread);
	CloseHandle(pi2.hThread);
	return 0;
}