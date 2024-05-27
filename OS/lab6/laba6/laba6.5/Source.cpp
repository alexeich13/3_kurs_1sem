#include <iostream>
#include <Windows.h>
#include <process.h>
#include <thread>
#include <mutex>
using namespace std;

int func()
{
	HANDLE hEvent = OpenEvent(EVENT_MODIFY_STATE, TRUE, L"Global\\EVNT");
	if (hEvent == NULL)
	{
		cerr << "Error: " << GetLastError() << " - event not open" << endl;
		return -1;
	}
	if (!ResetEvent(hEvent))
	{
		cerr << "Reset event false" << endl;
	}
	for (int i = 0; i < 90; i++)
	{
		if (i == 15)
		{
			if (!SetEvent(hEvent))
			{
				cerr << "Set event false" << endl;
			}
		}
		cout << i << " - " << "Current process: " << _getpid() << endl;
		this_thread::sleep_for(chrono::milliseconds(100));
	}

	CloseHandle(hEvent);

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

	LPCWSTR pathExe1 = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab6\\laba6\\x64\\Debug\\laba6.5a.exe";
	LPCWSTR pathExe2 = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab6\\laba6\\x64\\Debug\\laba6.5b.exe";

	HANDLE hEvent = CreateEvent(NULL, TRUE, FALSE, L"Global\\EVNT");
	if (hEvent == NULL)
	{
		cerr << "Error: " << GetLastError() << " - event not created" << endl;
		return -1;
	}
	else if (GetLastError() == ERROR_ALREADY_EXISTS)
	{
		cerr << "The event has already been created" << endl;
	}
	else cout << "New event created" << endl;
	pi1 = StartProcess(pathExe1);
	if (pi1.hProcess == NULL)
	{
		cerr << "Error: " << GetLastError() << " - the first process was not started" << endl;
	}
	else cout << "First process started" << endl;
	pi2 = StartProcess(pathExe2);
	if (pi2.hProcess == NULL)
	{
		cerr << "Error: " << GetLastError() << " - the second process was not started" << endl;
	}
	else cout << "Second process started" << endl;
	int resultFunc = func();
	if (resultFunc != 0)
	{
		cerr << "Error: " << GetLastError() << " - func() was not started" << endl;
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

	CloseHandle(hEvent);
	CloseHandle(pi1.hProcess);
	CloseHandle(pi2.hProcess);
	CloseHandle(pi1.hThread);
	CloseHandle(pi2.hThread);
	return 0;
}