using CTMS.Model.Entities.PageMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IPageMasterRepository
    {
        Task<int> I_ManagePage(PageMaster TBL);

        Task<int> U_ManagePage(PageMaster TBL);

        Task<int> D_ManagePage(PageMaster TBL);

        Task<List<PageMaster>> R_ManagePage(PageMaster TBL);
        Task<List<PageMaster>> RO_ManagePage(PageMaster TBL);
    }
}
