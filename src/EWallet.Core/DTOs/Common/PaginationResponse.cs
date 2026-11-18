namespace EWallet.Core.Dtos.Common;

public sealed record PaginationResponse<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);