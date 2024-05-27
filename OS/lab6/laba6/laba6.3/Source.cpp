#include <iostream>
#include <Windows.h>
#include <process.h>
#include <thread>
#include <mutex>
using namespace std;

int func() {
	HANDLE hMutex = OpenMutex(MUTEX_ALL_ACCESS, FALSE, L"MTX");
	if (hMutex == NULL)
	{
		cerr << "Error" << GetLastError() << " - mutex not open" << endl;
		return -1;
	}
	for (int i = 0; i < 90; i++)
	{
		if (i == 30)
		{
			WaitForSingleObject(hMutex, INFINITE);
		}
		else if (i == 60)
		{
			if (!ReleaseMutex(hMutex))
			{
				cout << "Relase mutex is false" << endl;
			}
		}
		cout << i << " - " << "Current process: " << _getpid() << endl;
		this_thread::sleep_for(chrono::milliseconds(100));
	}
	CloseHandle(hMutex);
	return 0;
}

int StartProcess(LPCWSTR pathExe1, LPCWSTR pathExe2)
{
	STARTUPINFO si1;
	PROCESS_INFORMATION pi1;

	ZeroMemory(&si1, sizeof(si1));
	si1.cb = sizeof(si1);
	ZeroMemory(&pi1, sizeof(pi1));

	STARTUPINFO si2;
	PROCESS_INFORMATION pi2;

	ZeroMemory(&si2, sizeof(si2));
	si2.cb = sizeof(si2);
	ZeroMemory(&pi2, sizeof(pi2));


	if (!CreateProcess(pathExe1, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si1, &pi1))
	{
		cerr << "Error: " << GetLastError() << " - the first process was not started" << endl;
		return -1;
	}
	else cout << "First process started" << endl;

	if (!CreateProcess(pathExe2, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si2, &pi2))
	{
		cerr << "Error: " << GetLastError() << " - the second process was not started" << endl;
		return -1;
	}
	else cout << "Second process started" << endl;

	if (WaitForSingleObject(pi1.hProcess, INFINITE) != WAIT_OBJECT_0)
	{
		cerr << "The first process has ended abnormaly" << endl;
	}
	else cout << "The first process has ended normaly" << endl;

	if (WaitForSingleObject(pi2.hProcess, INFINITE) != WAIT_OBJECT_0)
	{
		cerr << "The second process has ended abnormaly" << endl;
	}
	else cout << "The second process has ended normaly" << endl;

	CloseHandle(pi1.hProcess);
	CloseHandle(pi2.hProcess);
	CloseHandle(pi1.hThread);
	CloseHandle(pi2.hThread);
	return 0;
}

int main()
{
	LPCWSTR pathExe1 = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab6\\laba6\\x64\\Debug\\laba6.3a.exe";
	LPCWSTR pathExe2 = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab6\\laba6\\x64\\Debug\\laba6.3b.exe";
	HANDLE hMutex = CreateMutex(NULL, FALSE, L"MTX");
	if (hMutex == NULL)
	{
		cerr << "Error" << GetLastError() << " - mutex not created" << endl;
		return -1;
	}
	else if (GetLastError() == ERROR_ALREADY_EXISTS)
	{
		cerr << "The mutex has already been created" << endl;
	}
	else cout << "New mutex created" << endl;
	thread fm(func);
	int result = StartProcess(pathExe1, pathExe2);
	if (result != 0)
	{
		cerr << "Error: " << GetLastError() << " - StartProcess was not started" << endl;
	}
	CloseHandle(hMutex);
	fm.join();
	return result;
}