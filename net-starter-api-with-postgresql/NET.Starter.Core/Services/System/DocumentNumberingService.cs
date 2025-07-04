using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess;
using NET.Starter.Shared.Enums;

namespace NET.Starter.Core.Services.System
{
    /// <summary>
    /// Service for generating document numbers based on document type, reset period, and company/store codes.
    /// </summary>
    internal class DocumentNumberingService(ApplicationDbContext _dbContext, ILogger<DocumentNumberingService> _logger)
    {
        /// <summary>
        /// Generates a unique document number based on specified parameters.
        /// The number increments and resets based on the reset period.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="resetPeriod">Period when the numbering resets (e.g., monthly, yearly).</param>
        /// <param name="documentDate">Date of the document (used for period extraction).</param>
        /// <param name="companyCode">Company code included in the document number prefix.</param>
        /// <param name="storeCode">Store code included in the document number prefix.</param>
        /// <param name="padding">Number padding length (e.g., 5 means numbers like 00001).</param>
        /// <returns>The generated document number string.</returns>
        internal async Task<string> GenerateDocumentNumberAsync(
            DocumentType documentType
            , Period resetPeriod
            , DateOnly documentDate
            , string companyCode
            , string storeCode
            , int padding
        )
        {
            _logger.LogInformation(
                "Generating document number: Type={DocumentType}, Date={DocumentDate}, Reset={ResetPeriod}, Company={CompanyCode}, Store={StoreCode}",
                documentType, documentDate.ToString("yyyy-MM-dd"), resetPeriod, companyCode, storeCode
            );

            var periode = resetPeriod == Period.Monthly ? $"{documentDate.Year % 100:00}-{documentDate.Month:00}" : $"{documentDate.Year % 100:00}";
            var prefix = $"{documentType}-{periode}-{companyCode}-{storeCode}";

            _logger.LogInformation("Constructed prefix: {Prefix}", prefix);

            var documentNumbering = await _dbContext.DocumentNumberings.FirstOrDefaultAsync(d => EF.Functions.ILike(d.Prefix, prefix));

            if (documentNumbering is null)
            {
                _logger.LogInformation("Prefix not found. Creating new entry for: {Prefix}", prefix);

                documentNumbering = new()
                {
                    DocumentType = documentType,
                    ResetPeriode = resetPeriod,
                    Prefix = prefix,
                    LastNumber = 0,
                    Padding = padding
                };

                await _dbContext.AddAsync(documentNumbering);
            }
            else
            {
                _logger.LogInformation("Existing document numbering found for: {Prefix}", prefix);
            }

            documentNumbering.LastNumber++;
            var numberPadded = documentNumbering.LastNumber.ToString().PadLeft(documentNumbering.Padding, '0');
            var documentNumber = $"{prefix}-{numberPadded}";

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Generated document number: {DocumentNumber}", documentNumber);

            return documentNumber;
        }
    }
}
