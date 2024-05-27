#include <iostream>
#include <Windows.h>
using namespace std;


bool printFileInfo(LPCWSTR FileNameIn)
{
    cout << "Information about file:" << endl;

    WIN32_FIND_DATA fileData;

    HANDLE hFind = FindFirstFile(FileNameIn, &fileData);

    wcout << "Name: " << fileData.cFileName << endl;

    wcout << "Type: .txt" << endl;


    cout << "Size: " << fileData.nFileSizeLow << " bytes" << endl;
    FILETIME creationTime = fileData.ftCreationTime;
    FILETIME lastWriteTime = fileData.ftLastWriteTime;

    SYSTEMTIME sysCreationTime{}, sysLastAccessTime{}, sysLastWriteTime{};
    FileTimeToSystemTime(&creationTime, &sysCreationTime);
    FileTimeToSystemTime(&lastWriteTime, &sysLastWriteTime);

    cout << "Create time: " << sysCreationTime.wDay << "/" << sysCreationTime.wMonth << "/" << sysCreationTime.wYear << " "
        << sysCreationTime.wHour << ":" << sysCreationTime.wMinute << ":" << sysCreationTime.wSecond << endl;

    cout << "Last rewrite: " << sysLastWriteTime.wDay << "/" << sysLastWriteTime.wMonth << "/" << sysLastWriteTime.wYear << " "
        << sysLastWriteTime.wHour << ":" << sysLastWriteTime.wMinute << ":" << sysLastWriteTime.wSecond << endl;

    FindClose(hFind);
    return true;
}

bool printFileTxt(LPCWSTR FileNameIn)
{
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
        cout << endl << fileBuffer << endl << endl << endl;
    }
    delete[] fileBuffer;
    CloseHandle(hFile);
    return true;
}

int main()
{
    LPCWSTR fileName = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab9\\lab91.txt";
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
    return 0;
}