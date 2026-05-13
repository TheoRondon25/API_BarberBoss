using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberBoss.Application.UseCases.Billings.Reports.Excel;
public interface IGenerateBillingsReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
