﻿using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using SGF_ROHAN_WF.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = MigraDoc.DocumentObjectModel.Color;

namespace SGF_ROHAN_WF.Model
{
    public class PdfData
    {

        public Quotation ActiveQuotation { get; set; }
        public string Filename { get; set; }
        public string InternalName { get; set; }

        public Document ActiveDocument  { get; set; }

        public Section ActiveSection { get; set; }
        public Table TableData;

        public Color TableColor;
        public Color RohizColor;

        public PdfData(Quotation quotation, string filename)
        {
            ActiveDocument = new Document();
            ActiveQuotation = quotation;

            TableColor = Color.FromRgb(230, 240, 250);
            RohizColor = Color.FromRgb(252, 239, 189);

        }

        public void BuildDocumentStyles()
        {
            Style style = ActiveDocument.Styles["Normal"];

            style.Font.Name = "Verdana";

            style = ActiveDocument.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style = ActiveDocument.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            style = ActiveDocument.AddStyle("Table", "Normal");
            style.Font.Name = "Lucida Bright";
            style.Font.Size = 7;


            style = ActiveDocument.Styles[StyleNames.Normal];

        }

        //Vendor information
        public void BuildDocumentLayout()
        {
            ActiveSection = ActiveDocument.AddSection();

            Bitmap bmp = Resources.ROHIZ;
            bmp.Save("tempROHIZ.png");

            MigraDoc.DocumentObjectModel.Shapes.Image image = ActiveSection.Headers.Primary.AddImage("tempROHIZ.png");

            image.Height = "2cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Left;
            image.WrapFormat.Style = WrapStyle.Through;

            TextFrame VendorInfoTextFrame = ActiveSection.Headers.Primary.AddTextFrame();
            VendorInfoTextFrame.Height = "4cm";
            VendorInfoTextFrame.Width = "10cm";
            VendorInfoTextFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            VendorInfoTextFrame.Left = ShapePosition.Center;
            VendorInfoTextFrame.RelativeVertical = RelativeVertical.Line;
            VendorInfoTextFrame.Top = ShapePosition.Top;

            Paragraph VendorInfo = VendorInfoTextFrame.AddParagraph();
            VendorInfo.Style = "Table";
            VendorInfo.Format.Font.Size = 8;
            VendorInfo.Format.Alignment = ParagraphAlignment.Center;
            VendorInfo.AddText("VENTA Y CONFECCION DE CARPAS TOLVA");
            VendorInfo.AddLineBreak();
            VendorInfo.AddText("Sergio Rojas Delgado");
            VendorInfo.AddLineBreak();
            VendorInfo.AddText("Los Moreños #2846, Iquique");
            VendorInfo.AddLineBreak();
            VendorInfo.AddText("Cel. + 56942627155 / +56975591077");
        }

        //Generates client data based on the quotation
        public void GenerateClientInformation()
        {
            Paragraph para = ActiveSection.AddParagraph();
            para.Format.SpaceBefore = "1cm";

            Table ClientTable = ActiveSection.AddTable();

            ClientTable.Style = "Table";
            ClientTable.Borders.Width = 0.25;
            ClientTable.Borders.Left.Width = 0.25;
            ClientTable.Borders.Right.Width = 0.5;
            ClientTable.Rows.LeftIndent = 0;

            //Header fields
            Column column = ClientTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            //Client data fields
            column = ClientTable.AddColumn("6cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            //ClientName
            Row Row = ClientTable.AddRow();
            Row.Cells[0].AddParagraph("Cliente: ");
            Row.Cells[0].Shading.Color = RohizColor;
            Row.Cells[0].Format.Font.Bold = true;
            Row.Cells[1].AddParagraph(ActiveQuotation.Client.FullName);

            //Business
            Row = ClientTable.AddRow();
            Row.Cells[0].AddParagraph("Empresa: ");
            Row.Cells[0].Shading.Color = RohizColor;
            Row.Cells[0].Format.Font.Bold = true;
            Row.Cells[1].AddParagraph(ActiveQuotation.Client.RegisteredBusiness);

            //Email
            Row = ClientTable.AddRow();
            Row.Cells[0].AddParagraph("Email: ");
            Row.Cells[0].Shading.Color = RohizColor;
            Row.Cells[0].Format.Font.Bold = true;
            Row.Cells[1].AddParagraph(ActiveQuotation.Client.Email);

            //PhoneNumber
            Row = ClientTable.AddRow();
            Row.Cells[0].AddParagraph("Teléfono: ");
            Row.Cells[0].Shading.Color = RohizColor;
            Row.Cells[0].Format.Font.Bold = true;
            Row.Cells[1].AddParagraph(ActiveQuotation.Client.PhoneNumber);

        }

        //Generates the base table headers.
        public void GenerateTableHeaders()
        {

            Paragraph para = ActiveSection.AddParagraph();
            para.Format.SpaceBefore = "3cm";
            para.Style = "Table";
            para.Format.Alignment = ParagraphAlignment.Center;
            Text Greetings = para.AddText("A continuación presentamos nuestra oferta y esperamos que sea de su conformidad.");

            TableData = ActiveSection.AddTable();
            TableData.Style = "Table";
            TableData.Borders.Width = 0.25;
            TableData.Borders.Left.Width = 0.5;
            TableData.Borders.Right.Width = 0.5;
            TableData.Rows.LeftIndent = 0;

            //Item Column - 0
            Column column = TableData.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            //Quantity - 1
            column = TableData.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            //ProductName + Specs / Description - 2
            column = TableData.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            //UnitPrice - 3
            column = TableData.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            //TotalPrice - 4
            column = TableData.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            //Discount - 5
            column = TableData.AddColumn("1.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            //FinalPrice - 6
            column = TableData.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            Row row = TableData.AddRow();

            //Main Header Row
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = RohizColor;

            row.Cells[0].AddParagraph("Item");
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

            row.Cells[1].AddParagraph("Cantidad");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

            row.Cells[2].AddParagraph("Producto y Específicaciones");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;

            row.Cells[3].AddParagraph("Precio Unit.");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;

            row.Cells[4].AddParagraph("Precio Total");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Left;

            row.Cells[5].AddParagraph("Descuento");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[6].AddParagraph("Precio Final");
            row.Cells[6].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[6].VerticalAlignment = VerticalAlignment.Center;

            TableData.SetEdge(0, 0, 5, 1, Edge.Box, BorderStyle.Single, 0.5, Color.Empty);

        }

        //Creates rows based on the Quotation's Entries.
        public void PopulateTableData()
        {
            if(ActiveQuotation.ProductEntries.Count == 0)
            {
                return;
            }

            int i = 1;
            foreach(Entry entry in ActiveQuotation.ProductEntries)
            {
                //3 rows per entry

                //Item, Quantity, ProductName+Specs, UnitPrice, NetTotal, TotalPrice
                Row row1 = TableData.AddRow();

                //ProductDescription
                Row row2 = TableData.AddRow();

                //Item
                row1.TopPadding = 1.5;
                row1.Format.Alignment = ParagraphAlignment.Center;
                row1.Format.Font.Bold = true;
                row1.Cells[0].AddParagraph(i++.ToString());
                row1.Cells[0].MergeDown = 1;
                row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                //Quantity
                row1.Cells[1].AddParagraph(entry.Quantity.ToString() + " un.");
                row1.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[1].MergeDown = 1;

                //ProductName + Specs
                row1.Cells[2].AddParagraph(entry.RowProduct.ProductName + entry.RowProduct.ProductSpecifications);
                row1.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                row1.Shading.Color = Color.Empty;
                Border topborderinvis = new Border();
                topborderinvis.Visible = false;
                row1.Cells[2].Borders.Bottom = topborderinvis;
                

                //UnitPrice
                row1.Cells[3].AddParagraph("$" + entry.RowProduct.UnitPrice.ToString());
                row1.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                row1.Cells[3].MergeDown = 1;

                //TotalPrice
                row1.Cells[4].AddParagraph("$" + entry.TotalPrice.ToString());
                row1.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                row1.Cells[4].MergeDown = 1;

                //Discount
                row1.Cells[5].AddParagraph(entry.Discount.ToString() + "%");
                row1.Cells[5].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[5].MergeDown = 1;

                //TotalPrice
                row1.Cells[6].AddParagraph("$" + entry.FinalPrice.ToString());
                row1.Cells[6].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[6].VerticalAlignment = VerticalAlignment.Center;
                row1.Cells[6].MergeDown = 1;

                //ProductDescription
                row2.Cells[1].AddParagraph(entry.RowProduct.ProductDescription);
                row2.Cells[1].MergeDown = 1;
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                Border brd = new Border();
                brd.Visible = false;
                row2.Borders.Top = brd;
                row2.Borders.Style = BorderStyle.Single;
                row2.Shading.Color = Color.FromRgb(230, 230, 230);
                row2.Borders.Style = BorderStyle.Single;
                
            }


        }

        //Creates and populates the final price table.
        public void GeneratePriceTable()
        {
            Paragraph para = ActiveSection.AddParagraph();
            para.Format.SpaceBefore = "3cm";

            Table PriceTable = ActiveSection.AddTable();
            PriceTable.Style = "Table";
            PriceTable.Borders.Width = 0.25;
            PriceTable.Borders.Left.Width = 0.25;
            PriceTable.Borders.Right.Width = 0;
            PriceTable.Rows.LeftIndent = 0;
            PriceTable.Format.Alignment = ParagraphAlignment.Right;

            Column column = PriceTable.AddColumn("3cm");
            column = PriceTable.AddColumn("3cm");

            Row row = PriceTable.AddRow();

            row.Cells[0].AddParagraph("NETO");
            row.Cells[0].Shading.Color = RohizColor;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[1].AddParagraph("$" + ActiveQuotation.NetTotal.ToString());

            row = PriceTable.AddRow();
            row.Cells[0].AddParagraph("IVA");
            row.Cells[0].Shading.Color = RohizColor;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[1].AddParagraph("$" + ActiveQuotation.IvaDifference);

            row = PriceTable.AddRow();
            row.Cells[0].AddParagraph("TOTAL");
            row.Cells[0].Shading.Color = RohizColor;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[1].AddParagraph("$" + ActiveQuotation.TotalPrice);
        }


    }
}
