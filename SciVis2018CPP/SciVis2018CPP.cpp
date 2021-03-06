// SciVis2018CPP.cpp: Definiert den Einstiegspunkt für die Konsolenanwendung.
//

#include "stdafx.h"

#include <windows.h>

#include "vtkSphereSource.h"
#include "vtkPolyDataMapper.h"
#include "vtkActor.h"
#include "vtkRenderWindow.h"
#include "vtkRenderer.h"
#include "vtkRenderWindowInteractor.h"

int main()
{
		// create sphere geometry
		vtkSphereSource *sphere = vtkSphereSource::New();
		sphere->SetRadius(1.0);
		sphere->SetThetaResolution(18);
		sphere->SetPhiResolution(18);
	
		// map to graphics library
		vtkPolyDataMapper *map = vtkPolyDataMapper::New();
		vtkAlgorithmOutput* x = sphere->GetOutputPort();
		vtkPolyData* y = sphere->GetOutput(0);
		vtkInformation* z = sphere->GetOutputPortInformation(0);
		vtkDataObject* v = sphere->GetOutputDataObject(0);
		int w = sphere->GetOutputPointsPrecision();
		map->SetInputData(y);
	
		// actor coordinates geometry, properties, transformation
		vtkActor *aSphere = vtkActor::New();
		aSphere->SetMapper(map);
	
		// a renderer and render window
		vtkRenderer *ren1 = vtkRenderer::New();
		vtkRenderWindow *renWin = vtkRenderWindow::New();
		renWin->AddRenderer(ren1);
	
		// an interactor
		vtkRenderWindowInteractor *iren = vtkRenderWindowInteractor::New();
		iren->SetRenderWindow(renWin);
	
		// add the actor to the scene
		ren1->AddActor(aSphere);
		ren1->SetBackground(1, 1, 1); // Background color white
	
									  // render an image (lights and cameras are created automatically)
		renWin->Render();
	
		// begin mouse interaction
		iren->Start();
	
		return 0;
}


//int WINAPI WinMain(HINSTANCE hInstance,
//	HINSTANCE hPrevInstance,
//	LPSTR lpCmdLine,
//	int nCmdShow)
//{
//
//	// create sphere geometry
//	vtkSphereSource *sphere = vtkSphereSource::New();
//	sphere->SetRadius(1.0);
//	sphere->SetThetaResolution(18);
//	sphere->SetPhiResolution(18);
//
//	// map to graphics library
//	vtkPolyDataMapper *map = vtkPolyDataMapper::New();
//	map->SetInput(sphere->GetOutput());
//
//	// actor coordinates geometry, properties, transformation
//	vtkActor *aSphere = vtkActor::New();
//	aSphere->SetMapper(map);
//
//	// a renderer and render window
//	vtkRenderer *ren1 = vtkRenderer::New();
//	vtkRenderWindow *renWin = vtkRenderWindow::New();
//	renWin->AddRenderer(ren1);
//
//	// an interactor
//	vtkRenderWindowInteractor *iren = vtkRenderWindowInteractor::New();
//	iren->SetRenderWindow(renWin);
//
//	// add the actor to the scene
//	ren1->AddActor(aSphere);
//	ren1->SetBackground(1, 1, 1); // Background color white
//
//								  // render an image (lights and cameras are created automatically)
//	renWin->Render();
//
//	// begin mouse interaction
//	iren->Start();
//
//	return 0;
//}

