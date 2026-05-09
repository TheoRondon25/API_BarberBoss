using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Requests;
public class RequestGetAllBillingsJson
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? BarberName { get; set; }
    public string? ClientName { get; set; }
    public string? ServiceName { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public string OrderBy { get; set; } = "Date";
    public bool Descending { get; set; } = true;
}
