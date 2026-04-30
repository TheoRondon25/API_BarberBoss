using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Responses;
public class ResponseShortBillingsJson
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public string BarberName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }    
}
