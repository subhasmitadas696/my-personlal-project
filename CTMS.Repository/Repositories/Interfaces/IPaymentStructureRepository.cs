using CTMS.Model.Entities.AdminMaster;
using CTMS.Model.Entities.PaymentStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Repository.Repositories.Interfaces
{
    public  interface IPaymentStructureRepository
    {
        Task<int> InsertPaymentStructure(List<PaymentStructureMaster> PaymentStructure,string Code);
        Task<List<PaymentStructureMaster>> ViewPaymentStructure();
        Task<int> DeletePaymentStructure(int Id);
        Task<List<PaymentStructureMaster>> PaymentStructureGetById(int Id);
    }
}
