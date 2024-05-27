
// ChildView.cpp: реализация класса CChildView
//

#include "stdafx.h"
#include "Lab01.h"
#include "ChildView.h"

#ifdef _DEBUG
//#define new DEBUG_NEW
#endif


// CChildView

CChildView::CChildView()
{												
	CMatrix A(3, 3), B(3, 3), V1(3), V2(3), V3(3);					
	controller.InitMatrix(A); controller.InitMatrix(B);		
	controller.InitMatrix(V1); controller.InitMatrix(V2);
	arr.push_back(A); arr.push_back(B); arr.push_back(V1);	
	arr.push_back(V2); arr.push_back(V3);
}

CChildView::~CChildView()
{
}


BEGIN_MESSAGE_MAP(CChildView, CWnd)
	ON_WM_PAINT()
	ON_COMMAND(ID_TEST_MATRIX, &CChildView::OnTestMatrix)			
	ON_COMMAND(ID_TEST_FUNCTIONS, &CChildView::OnTestFunctions)		
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
}

											
void CChildView::OnTestMatrix()
{
	CClientDC dc(this);									
	InvalidateRect(0);
	UpdateWindow();										
	CPen MyPen(PS_DASHDOT, 4, RGB(133, 255, 100));		
	dc.TextOut(10, 5, _T("Исходные матрицы:"));			
	dc.TextOut(10, 70, _T("A = "));
	controller.PrintMatrix(dc, 40, 30, arr[0]);			
	dc.TextOut(10, 240, _T("B = "));
	controller.PrintMatrix(dc, 40, 200, arr[1]);
	dc.TextOut(250, 5, _T("Исходные векторы:"));
	dc.TextOut(250, 70, _T("V1 = "));
	controller.PrintMatrix(dc, 285, 30, arr[2]);
	dc.TextOut(330, 70, _T("V2 = "));
	controller.PrintMatrix(dc, 365, 30, arr[3]);
	dc.TextOut(410, 70, _T("V3 = "));
	controller.PrintMatrix(dc, 445, 30, arr[4]);

	dc.TextOut(500, 5, _T("Результат:"));
	CMatrix C1 = arr[0] + arr[1];
	dc.TextOut(500, 30, _T("C1 = A + B"));
	controller.PrintMatrix(dc, 500, 50, C1);

	CMatrix C2 = arr[0] * arr[1];
	dc.TextOut(700, 30, _T("C2 = A * B"));
	controller.PrintMatrix(dc, 700, 50, C2);

	CMatrix D = arr[0] * arr[2];
	dc.TextOut(900, 30, _T("D = A * V1"));
	controller.PrintMatrix(dc, 900, 50, D);

	CMatrix q = arr[2].Transp() * arr[3];
	dc.TextOut(500, 200, _T("q = V1^T * V2"));
	controller.PrintMatrix(dc, 500, 250, q);

	CMatrix p = arr[2].Transp() * arr[0] * arr[3];
	dc.TextOut(700, 200, _T("p = V1^T * A * V2"));
	controller.PrintMatrix(dc, 700, 250, p);
}

											
void CChildView::OnTestFunctions()
{
	CClientDC dc(this);
	InvalidateRect(0);
	UpdateWindow();								
	dc.TextOut(10, 5, _T("Исходные векторы:")); 
	dc.TextOut(10, 70, _T("V1 = "));
	controller.PrintMatrix(dc, 45, 30, arr[2]);
	dc.TextOut(85, 70, _T("V2 = "));
	controller.PrintMatrix(dc, 120, 30, arr[3]);
	



	dc.TextOut(200, 5, _T("Векторное произведение:"));
	auto vec = controller.VectorMult(arr[2], arr[3]);
	controller.PrintMatrix(dc, 200, 30, vec);

	dc.TextOut(420, 5, _T("Скалярное произведение:"));
	auto stringScal = controller.DoubleToString(controller.ScalarMult(arr[2], arr[3]));
	dc.TextOut(420, 30, stringScal);

	dc.TextOut(420, 70, _T("Модуль вектора V1:"));
	auto stringMod = controller.DoubleToString(controller.ModVec(arr[2]));
	dc.TextOut(420, 95, stringMod);

	dc.TextOut(420, 135, _T("Косинус между V1 и V2:"));
	auto stringCos = controller.DoubleToString(controller.CosV1V2(arr[2], arr[3]));
	dc.TextOut(420, 160, stringCos);

	dc.TextOut(10, 200, _T("Преобразует сферические координаты PView точки в декартовы:"));
	auto v = CMatrix(3);
	controller.InitMatrix(v);
	dc.TextOut(10, 265, _T("PView = "));
	controller.PrintMatrix(dc, 70, 225, v);
	controller.PrintMatrix(dc, 300, 225, controller.SphereToCart(v));

}