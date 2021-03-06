#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace Sesion01
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            string fileName = doc.PathName;
            
            double offset = 0.02;                                   // Always Imperial ??
            double offsetScale = offset * doc.ActiveView.Scale;

            XYZ curPoint = new XYZ(0, 0, 0);
            XYZ offsetPoint = new XYZ(0, offsetScale, 0);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(TextNoteType));

            Transaction t = new Transaction(doc, "Create Text Note");
            t.Start();

            int range = 100;
            for (int i = 1; i <= range; i++)
            {
                TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id, curPoint, "This is line " + i.ToString(), collector.FirstElementId());
                curPoint = curPoint.Subtract(offsetPoint);
            }

            t.Commit();
            t.Dispose();

            return Result.Succeeded;
        }
    }
}
