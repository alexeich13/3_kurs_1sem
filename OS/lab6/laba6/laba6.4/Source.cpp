#include <iostream>
#include <Windows.h>
#include <process.h>
#include <thread>
#include <mutex>
using namespace std;

int func()
{
	HANDLE hSemaphore = OpenSemaphore(SEMAPHORE_ALL_ACCESS, FALSE, L"Global\\SEMPH");
	if (hSemaphore == NULL)
	{
		cerr << "Error: " << GetLastError() << " - semaphore not open" << endl;
		return -3;
	}

	for (int i = 0; i < 90; i++)
	{
		if (i == 30)
		{
			WaitForSingleObject(hSemaphore, INFINITE);
		}
		else if (i == 60)
		{
			if (!ReleaseSemaphore(hSemaphore, 2, NULL))
			{
				cout << "Relase semaphore is false" << endl;
			}
		}
		cout << i << " - " << "Current process: " << _getpid() << endl;
		this_thread::sleep_for(chrono::milliseconds(100));
	}
	CloseHandle(hSemaphore);
	return 0;
}


PROCESS_INFORMATION StartProcess(LPCWSTR pathExe1)
{
	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));


	if (!CreateProcess(pathExe1, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &pi))
	{
		cerr << "Error: " << GetLastError() << " - CreatProcess was not started" << endl;
	}

	return pi;
}

int main()
{
	PROCESS_INFORMATION pi1;
	PROCESS_INFORMATION pi2;
	LPCWSTR pathExe1 = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab6\\laba6\\x64\\Debug\\laba6.4a.exe";
	LPCWSTR pathExe2 = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab6\\laba6\\x64\\Debug\\laba6.4b.exe";
	HANDLE hSemaphore = CreateSemaphore(NULL, 2, 3, L"Global\\SEMPH");
	if (hSemaphore == NULL)
	{
		cerr << "Error: " << GetLastError() << " - semaphore not created" << endl;
		CloseHandle(hSemaphore);
		return -1;
	}
	else if (GetLastError() == ERROR_ALREADY_EXISTS)
	{
		cerr << "The semaphore has already been created" << endl;
	}
	else cout << "New semaphore created" << endl;
	pi1 = StartProcess(pathExe1);
	if (pi1.hProcess == NULL)
	{
		cerr << "Error: " << GetLastError() << " - the first process was not started" << endl;
		return -1;
	}
	else cout << "First process started" << endl;
	pi2 = StartProcess(pathExe2);
	if (pi2.hProcess == NULL)
	{
		cerr << "Error: " << GetLastError() << " - the second process was not started" << endl;
		return -1;
	}
	else cout << "Second process started" << endl;
	int resultFunc = func();
	if (resultFunc != 0)
	{
		cerr << "Error: " << GetLastError() << " - func() was not started" << endl;
		return -2;
	}
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
	CloseHandle(hSemaphore);
	CloseHandle(pi1.hProcess);
	CloseHandle(pi2.hProcess);
	CloseHandle(pi1.hThread);
	CloseHandle(pi2.hThread);
	return 0;
}