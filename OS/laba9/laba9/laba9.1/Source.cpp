#include <iostream>
#include <Windows.h>
using namespace std;


bool printFileInfo(LPCWSTR FileNameIn)
{
    cout << "Reading information about a file..." << endl << endl;

    WIN32_FIND_DATA fileData;

    HANDLE hFind = FindFirstFile(FileNameIn, &fileData);

    wcout << "File Name: " << fileData.cFileName << endl;

    if (fileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
        wcout << "File Type: Directory" << endl;
    else
        wcout << "File Type: File" << endl;

    cout << "File size: " << fileData.nFileSizeLow << " bytes" << endl;
    FILETIME creationTime = fileData.ftCreationTime;
    FILETIME lastWriteTime = fileData.ftLastWriteTime;

    SYSTEMTIME sysCreationTime{}, sysLastAccessTime{}, sysLastWriteTime{};
    FileTimeToSystemTime(&creationTime, &sysCreationTime);
    FileTimeToSystemTime(&lastWriteTime, &sysLastWriteTime);

    cout << "File creation time: " << sysCreationTime.wDay << "/" << sysCreationTime.wMonth << "/" << sysCreationTime.wYear << " "
        << sysCreationTime.wHour << ":" << sysCreationTime.wMinute << ":" << sysCreationTime.wSecond << endl;

    cout << "File last write time: " << sysLastWriteTime.wDay << "/" << sysLastWriteTime.wMonth << "/" << sysLastWriteTime.wYear << " "
        << sysLastWriteTime.wHour << ":" << sysLastWriteTime.wMinute << ":" << sysLastWriteTime.wSecond << endl << endl;

    FindClose(hFind);
    cout << "Reading information about a file completed." << endl;
    return true;
}

bool printFileTxt(LPCWSTR FileNameIn)
{
    cout << "Reading data from a file..." << endl << endl;

    HANDLE hFile = CreateFile(FileNameIn, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

    DWORD fileSize = GetFileSize(hFile, NULL);

    char* fileBuffer = new char[fileSize + 1];

    DWORD bytesRead = 0;
    DWORD  dwBytesRead = 0;
    if (!ReadFile(hFile, fileBuffer, fileSize, &bytesRead, NULL))
    {
        cerr << "Error: " << GetLastError() << " - error reading file." << endl;
        cout << "Reading data from file was terminated." << endl;
        delete[] fileBuffer;
        CloseHandle(hFile);
        return false;
    }

    fileBuffer[bytesRead] = '\0';

    if (bytesRead == 0)
    {
        cout << "Warning: File empty." << endl << endl;
    }
    else
    {
        cout << "<<<" << endl << fileBuffer << endl << ">>>" << endl << endl;
    }

    delete[] fileBuffer;
    CloseHandle(hFile);

    cout << "Reading data from file completed." << endl;
    return true;
}

int main()
{
    LPCWSTR fileName = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\laba9\\laba93.txt";

    if (!printFileInfo(fileName))
    {
        cerr << "Error: " << GetLastError() << " - read text file info failed. " << endl;
        return 2;
    }

    if (!printFileTxt(fileName))
    {
        cerr << "Error: " << GetLastError() << " - read file text failed. " << endl;
        return 2;
    }

    cout << "Press Enter to exit: ";
    cin.get();

    return 0;
}