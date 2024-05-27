
// ChildView.cpp: реализация класса CChildView
//

#include "pch.h"
#include "framework.h"
#include "Lab_2.h"
#include "ChildView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif
#define pi 3.14159265358979323846;

CChildView::CChildView()
{
}

CChildView::~CChildView()
{
}


BEGIN_MESSAGE_MAP(CChildView, CWnd)
	ON_WM_PAINT()
	ON_COMMAND(ID_TESTS_TEST, &CChildView::OnTestsF1)
	ON_COMMAND(ID_TESTS_F2, &CChildView::OnTestsF2)
	ON_COMMAND(ID_TESTS_F12, &CChildView::OnTestsF12)
END_MESSAGE_MAP()

BOOL CChildView::PreCreateWindow(CREATESTRUCT& cs) 
{
	if (!CWnd::PreCreateWindow(cs))
		return FALSE;

	cs.dwExStyle |= WS_EX_CLIENTEDGE;
	cs.style &= ~WS_BORDER;
	cs.lpszClass = AfxRegisterWndClass(CS_HREDRAW|CS_VREDRAW|CS_DBLCLKS, 
		::LoadCursor(nullptr, IDC_ARROW), reinterpret_cast<HBRUSH>(COLOR_WINDOW+1), nullptr);

	return TRUE;
}

void CChildView::OnPaint() 
{
	CPaintDC dc(this); 

	if (Index == 1)		
	{
		Graph.Draw(dc, 0, 0);
	}
}

double CChildView::MyF1(double x)
{
	double y = sin(x) / x;
	return y;
}

double CChildView::MyF2(double x)
{
	double y = sqrt(x) * sin(x);
	return y;
}

void CChildView::OnTestsF1()	
{
	double Xl = -3 * pi;		
	double Xh = -Xl;			
	double dX = 3.14159265358979323846 / 36;		
	int N = (Xh - Xl) / dX;		
	X.RedimMatrix(N + 1);		
	Y.RedimMatrix(N + 1);		
	for (int i = 0; i <= N; i++)
	{
		X(i) = Xl + i * dX;		
		Y(i) = MyF1(X(i));
	}
	PenLine.Set(PS_SOLID, 1, RGB(255, 0, 0));	
	PenAxis.Set(PS_SOLID, 2, RGB(0, 0, 255));	
	RW.SetRect(100, 100, 500, 500);				
	Graph.SetParams(X, Y, RW);					
	Graph.SetPenLine(PenLine);					
	Graph.SetPenAxis(PenAxis);					
	Index = 1;									
	this->Invalidate();
}

void CChildView::OnTestsF2()
{
	double Xl = 0;
	double Xh = 6 * pi;
	double dX = 3.14159265358979323846 / 36;
	int N = (Xh - Xl) / dX;
	X.RedimMatrix(N + 1);
	Y.RedimMatrix(N + 1);
	for (int i = 0; i <= N; i++)
	{
		X(i) = Xl + i * dX;
		Y(i) = MyF2(X(i));
	}
	PenLine.Set(PS_DASHDOT, 1, RGB(255, 0, 0));		
	PenAxis.Set(PS_SOLID, 2, RGB(0, 0, 0));
	RW.SetRect(100, 100, 500, 500);
	Graph.SetParams(X, Y, RW);
	Graph.SetPenLine(PenLine);
	Graph.SetPenAxis(PenAxis);
	Index = 1;
	this->Invalidate();
}


void CChildView::OnTestsF12()
{
	Invalidate();
	CPaintDC dc(this);
	double Xl = -3 * pi;
	double Xh = -Xl;
	double dX = 3.14159265358979323846 / 36;
	int N = (Xh - Xl) / dX;
	X.RedimMatrix(N + 1);
	Y.RedimMatrix(N + 1);
	for (int i = 0; i <= N; i++)
	{
		X(i) = Xl + i * dX;
		Y(i) = MyF1(X(i));
	}
	PenLine.Set(PS_SOLID, 1, RGB(255, 0, 0));
	PenAxis.Set(PS_SOLID, 2, RGB(0, 0, 255));
	RW.SetRect(20, 10, 270, 260);
	Graph.SetParams(X, Y, RW);
	Graph.SetPenLine(PenLine);
	Graph.SetPenAxis(PenAxis);
	Graph.Draw(dc, 1, 1);

	Xl = 0;
	Xh = 6 * pi;
	dX = 3.14159265358979323846 / 36;
	N = (Xh - Xl) / dX;
	X.RedimMatrix(N + 1);
	Y.RedimMatrix(N + 1);
	for (int i = 0; i <= N; i++)
	{
		X(i) = Xl + i * dX;
		Y(i) = MyF2(X(i));
	}
	PenLine.Set(PS_DASHDOT, 1, RGB(255, 0, 0));
	PenAxis.Set(PS_SOLID, 2, RGB(0, 0, 0));
	RW.SetRect(400, 10, 650, 260);
	Graph.SetParams(X, Y, RW);
	Graph.SetPenLine(PenLine);
	Graph.SetPenAxis(PenAxis);
	Graph.Draw(dc, 1, 1);
}