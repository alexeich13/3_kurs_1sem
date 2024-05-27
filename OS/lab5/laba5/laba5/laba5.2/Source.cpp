#include <iostream>
#include <Windows.h>
using namespace std;

DWORD_PTR CountToAffinityMask(int affinityMask)
{
	DWORD_PTR affinityMaskBit = 0;
	for (int i = 0; i < affinityMask; i++) {
		affinityMaskBit |= (static_cast<unsigned long long>(1) << i);
	}
	return affinityMaskBit;
}

int StartProcess(int affinityMask, int firstProccessPriority, int secondProccessPriority)
{
	DWORD_PTR affinityMaskDWORD = CountToAffinityMask(affinityMask);
	DWORD_PTR firstProcPriority = static_cast<DWORD_PTR>(firstProccessPriority);
	DWORD_PTR secondProcPriority = static_cast<DWORD_PTR>(secondProccessPriority);

	LPCWSTR exeOne = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab5\\laba5\\laba5\\x64\\Debug\\laba5.2x.exe";
	LPCWSTR exeTwo = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab5\\laba5\\laba5\\x64\\Debug\\laba5.2x.exe";

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

	if (!SetProcessAffinityMask(GetCurrentProcess(), affinityMaskDWORD))
	{
		cerr << "Error: " << GetLastError() << " - the affinity mask was not processed" << endl;
	}

	if (!CreateProcess(exeOne, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE | firstProcPriority, NULL, NULL, &si1, &pi1))
	{
		cerr << "Error: " << GetLastError() << " - the first process was not started" << endl;
	}
	else
		cout << "First proccess started" << endl;

	if (!CreateProcess(exeTwo, NULL, NULL, NULL, FALSE, CREATE_NEW_CONSOLE | secondProcPriority, NULL, NULL, &si2, &pi2))
	{
		cerr << "Error: " << GetLastError() << " - the second process was not started" << endl;;
	}
	else
		cout << "Second proccess started" << endl;

	WaitForSingleObject(pi1.hProcess, INFINITE);
	WaitForSingleObject(pi2.hProcess, INFINITE);

	CloseHandle(pi1.hProcess);
	CloseHandle(pi1.hThread);
	CloseHandle(pi2.hProcess);
	CloseHandle(pi2.hThread);
	return 0;
}

int main()
{
	int affinityMask;
	int firstProccessPriority;
	int secondProccessPriority;
	DWORD_PTR maxProccessCount = GetMaximumProcessorCount(ALL_PROCESSOR_GROUPS);

	cout << "Number of available processors: " << maxProccessCount << endl;
	cout << "Enter the number of available processors for the process: "; 
	cin >> affinityMask;
	cout << "Enter first proccess priority: ";
	cin >> firstProccessPriority;
	cout << "Enter second proccess priority: ";
	cin >> secondProccessPriority;

	if (StartProcess(affinityMask, firstProccessPriority, secondProccessPriority) != 0)
	{
		cerr << "Error: " << GetLastError() << " - Start proccess failed" << endl;
	}
	else
	{
		cout << "Start Proccess completed" << endl;
	}

	return 0;
}