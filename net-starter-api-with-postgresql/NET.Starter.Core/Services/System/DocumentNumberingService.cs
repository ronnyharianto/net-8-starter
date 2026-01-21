using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Starter.DataAccess;
using NET.Starter.Shared.Enums;
using System.Text.RegularExpressions;

namespace NET.Starter.Core.Services.System
{
    /// <summary>
    /// Service for generating document numbers based on document type, reset period, and company/store codes.
    /// </summary>
    internal class DocumentNumberingService(ApplicationDbContext _dbContext, ILogger<DocumentNumberingService> _logger)
    {
        /// <summary>
        /// Generates a unique document number based on the configured
        /// <see cref="DocumentNumbering"/> for the specified <see cref="DocumentType"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The generated number is sequential and resets based on the
        /// configured reset period (e.g. monthly or yearly).
        /// </para>
        /// <para>
        /// The document number prefix may contain dynamic placeholders, such as:
        /// </para>
        /// <list type="bullet">
        ///   <item>
        ///     <description>
        ///     Metadata placeholders: <c>[Key]</c> (replaced using <paramref name="metadatas"/>)
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///     Date placeholders: <c>[Date:format]</c>
        ///     (e.g. <c>[Date:yyyyMM]</c>)
        ///     </description>
        ///   </item>
        /// </list>
        /// </remarks>
        /// <param name="documentType">
        /// The type of document for which the number is generated.
        /// </param>
        /// <param name="documentDate">
        /// The document date used to determine the reset period
        /// and to resolve date placeholders in the prefix.
        /// </param>
        /// <param name="metadatas">
        /// A collection of key-value pairs used to replace metadata
        /// placeholders in the document number prefix.
        /// </param>
        /// <returns>
        /// The generated document number string.
        /// Returns an empty string if the configuration is not found
        /// or the reset period is unsupported.
        /// </returns>
        internal async Task<string> GenerateDocumentNumberAsync(
            DocumentType documentType
            , DateOnly documentDate
            , Dictionary<string, string> metadatas
        )
        {
            _logger.LogInformation(
                "Generating document number: Type={DocumentType}, Date={DocumentDate}, Metadata={@Metadata}",
                documentType, documentDate.ToString("yyyy-MM-dd"), metadatas
            );

            var documentNumbering = await _dbContext.DocumentNumberings.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentType == documentType);
            if (documentNumbering is null)
                return string.Empty;

            DateOnly? periodKey = default;
            if (documentNumbering.ResetPeriode == Period.Yearly)
                periodKey = new(documentDate.Year, 1, 1);
            else if (documentNumbering.ResetPeriode == Period.Monthly)
                periodKey = new(documentDate.Year, documentDate.Month, 1);

            if (periodKey is null)
                return string.Empty;

            var documentNumberingCounter = await _dbContext.DocumentNumberingCounters.FirstOrDefaultAsync(d => d.DocumentNumberingId == documentNumbering.Id && d.PeriodKey == periodKey);
            if (documentNumberingCounter is null)
            {
                documentNumberingCounter = new()
                {
                    DocumentNumberingId = documentNumbering.Id,
                    PeriodKey = periodKey.Value,
                    LastNumber = 0
                };

                await _dbContext.AddAsync(documentNumberingCounter);
            }

            documentNumberingCounter.LastNumber++;

            var documentNumber = $"{documentNumbering.Prefix}";

            foreach (var metadata in metadatas)
            {
                documentNumber = documentNumber.Replace(
                    $"[{metadata.Key}]",
                    metadata.Value,
                    StringComparison.OrdinalIgnoreCase
                );
            }

            var dateMatches = Regex.Matches(
                documentNumber,
                @"\[Date:(?<format>[^\]]+)\]",
                RegexOptions.IgnoreCase
            );

            foreach (Match match in dateMatches)
            {
                var format = match.Groups["format"].Value;
                var formatted = documentDate
                    .ToDateTime(TimeOnly.MinValue)
                    .ToString(format);

                documentNumber = documentNumber.Replace(match.Value, formatted);
            }

            documentNumber = $"{documentNumber}{documentNumberingCounter.LastNumber.ToString().PadLeft(documentNumbering.Padding, '0')}";

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Generated document number: {DocumentNumber}", documentNumber);

            return documentNumber;
        }
    }
}
