#include <iostream>
#include <Windows.h>
#include <process.h>
#include <mutex>

using namespace std;

int main()
{
	HANDLE hEvent = OpenEvent(SYNCHRONIZE, FALSE, L"Global\\EVNT");
	if (hEvent == NULL)
	{
		cerr << "Error: " << GetLastError() << " - event not open" << endl;
		return -1;
	}

	if (WaitForSingleObject(hEvent, INFINITE) != WAIT_OBJECT_0)
	{
		cerr << "Error" << GetLastError() << " - wait event failed" << endl;
	}

	for (int i = 0; i < 90; i++)
	{
		cout << i << " - " << "Current process: " << _getpid() << endl;
		this_thread::sleep_for(chrono::milliseconds(100));
	}

	CloseHandle(hEvent);

	return 0;
}