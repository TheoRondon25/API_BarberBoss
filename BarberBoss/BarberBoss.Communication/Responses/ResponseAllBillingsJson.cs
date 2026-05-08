using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberBoss.Communication.Responses;
public class ResponseAllBillingsJson
{
    public List<ResponseShortBillingsJson> Billings { get; set; } = [];
}
