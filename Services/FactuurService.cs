using iTextSharp.text;
using iTextSharp.text.pdf;
using WPR_project.Repositories;

namespace WPR_project.Services
{
    public class FactuurService
    {
        private readonly IWagenparkBeheerderRepository _wagenparkBeheerderRepository;
        private readonly IAbonnementRepository _abonnementRepository;
        private readonly IFactuurRepository _factuurRepository;
        private readonly IZakelijkeHuurderRepository _zakelijkeHuurderRepository;

        public FactuurService(IWagenparkBeheerderRepository wagenparkBeheerderRepository,
                              IAbonnementRepository abonnementRepository,
                              IFactuurRepository factuurRepository,
                              IZakelijkeHuurderRepository zakelijkeHuurderRepository)
        {
            _wagenparkBeheerderRepository = wagenparkBeheerderRepository;
            _abonnementRepository = abonnementRepository;
            _factuurRepository = factuurRepository;
            _zakelijkeHuurderRepository = zakelijkeHuurderRepository;
        }

        public byte[] GenerateInvoicePDF(Guid beheerderId, Guid abonnementId)
        {
            var beheerder = _wagenparkBeheerderRepository.GetBeheerderById(beheerderId);
            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            var zakelijkeId = _wagenparkBeheerderRepository.GetZakelijkeId(beheerderId);
            var bedrijf = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            Guid factuurId = Guid.NewGuid();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                PdfPTable headerTable = new PdfPTable(2)
                {
                    WidthPercentage = 100
                };
                headerTable.SetWidths(new float[] { 1f, 3f });

                try
                {
                    string logoPath = "Views/src/assets/CarAndAll_Logo.jpg";
                    Image logo = Image.GetInstance(logoPath);
                    logo.ScalePercent(15);
                    PdfPCell logoCell = new PdfPCell(logo)
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        PaddingBottom = 50
                    };
                    headerTable.AddCell(logoCell);
                }
                catch
                {
                    headerTable.AddCell(new Phrase("Logo niet beschikbaar"));
                }

                PdfPCell infoCell = new PdfPCell(new Phrase(
                    "CarAndAll\nJohanna Westerdijkplein 75\n2521 EP Den Haag\nNederland\n" +
                    "Telefoon: 070 445 8888\nEmail: noreply.carandall@gmail.com\nWebsite: CarAndAll.net",
                    new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_TOP,
                    PaddingLeft = 250,
                    PaddingTop = 5
                };
                headerTable.AddCell(infoCell);

                document.Add(headerTable);




                PdfPTable infoTable = new PdfPTable(2)
                {
                    WidthPercentage = 100
                };
                infoTable.SetWidths(new float[] { 1f, 2f });

                infoTable.AddCell(new PdfPCell(new Phrase("FactuurId:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase(factuurId.ToString(), new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });

                infoTable.AddCell(new PdfPCell(new Phrase("Datum:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd-MM-yyyy"), new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase("BedrijfsNaam:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase(bedrijf.bedrijfsNaam, new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase("KVK nummer:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase(bedrijf.KVKNummer.ToString(), new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });

                infoTable.AddCell(new PdfPCell(new Phrase("KantoorAdres:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase(bedrijf.adres, new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase("Telefoonnummer:", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });
                infoTable.AddCell(new PdfPCell(new Phrase(bedrijf.telNummer, new Font(Font.FontFamily.HELVETICA, 10)))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                });

                document.Add(infoTable);

                // Factuurtabel
                PdfPTable itemTable = new PdfPTable(new float[] { 1f, 2.5f, 1f, 1f })
                {
                    WidthPercentage = 100,
                    SpacingBefore = 20
                };

                itemTable.AddCell(new PdfPCell(new Phrase("Aantal", FontFactory.GetFont("HELVETICA", 10, Font.BOLD)))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                itemTable.AddCell(new PdfPCell(new Phrase("Omschrijving", FontFactory.GetFont("HELVETICA", 10, Font.BOLD)))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                itemTable.AddCell(new PdfPCell(new Phrase("Prijs per eenheid", FontFactory.GetFont("HELVETICA", 10, Font.BOLD)))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                itemTable.AddCell(new PdfPCell(new Phrase("Bedrag", FontFactory.GetFont("HELVETICA", 10, Font.BOLD)))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                itemTable.AddCell(new PdfPCell(new Phrase("1")));
                itemTable.AddCell(new PdfPCell(new Phrase(abonnement.Naam)));
                itemTable.AddCell(new PdfPCell(new Phrase($"€ {abonnement.Kosten:F2}")));
                itemTable.AddCell(new PdfPCell(new Phrase($"€ {abonnement.Kosten:F2}")));

                document.Add(itemTable);

                // Betaalinformatie
                Paragraph paymentInfo = new Paragraph(
                    $"Betaal binnen 14 dagen na factuurdatum (vóór {DateTime.Now.AddDays(14):dd-MM-yyyy}) naar NL18RABO0115283327.\n" +
                    "Vermeld uw factuurId en Bedrijfsnaam bij de banktransactie.",
                    new Font(Font.FontFamily.HELVETICA, 10))
                {
                    SpacingBefore = 20,
                    Alignment = Element.ALIGN_LEFT
                };
                document.Add(paymentInfo);

                document.Close();

                // Opslaan van de factuur in de database
                var factuur = new Factuur
                {
                    FactuurId = factuurId,
                    BeheerderId = beheerderId,
                    AbonnementId = abonnementId,
                    FactuurPDF = memoryStream.ToArray(),
                    FactuurDatum = DateTime.Now
                };
                _factuurRepository.SaveInvoice(factuur);

                return memoryStream.ToArray();
            }
        }
    }
}
