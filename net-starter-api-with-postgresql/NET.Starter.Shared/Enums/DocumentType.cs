namespace NET.Starter.Shared.Enums
{
    /// <summary>
    /// Defines supported document types in the system.
    /// Typically used to categorize transactional records.
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// A document representing a customer sales order.
        /// </summary>
        SalesOrder,

        /// <summary>
        /// A document representing a supplier purchase order.
        /// </summary>
        PurchaseOrder,

        /// <summary>
        /// A document representing an issued invoice.
        /// </summary>
        Invoice
    }
}