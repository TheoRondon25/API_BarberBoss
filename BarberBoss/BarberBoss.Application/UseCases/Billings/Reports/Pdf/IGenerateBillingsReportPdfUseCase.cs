using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberBoss.Application.UseCases.Billings.Reports.Pdf;
public interface IGenerateBillingsReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly date);
}
