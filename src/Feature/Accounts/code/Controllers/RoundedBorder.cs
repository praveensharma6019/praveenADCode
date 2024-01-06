extern alias itextsharp;
using itextsharp::iTextSharp.text;
using itextsharp::iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Controllers
{
    internal class RoundedBorder : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, Rectangle rect, PdfContentByte[] canvas)
        {
            PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
            cb.RoundRectangle(
              rect.Left + 1.5f,
              rect.Bottom + 1.5f,
              rect.Width - 3,
              rect.Height - 2, 4
            );
            cb.Stroke();
        }
    }
}