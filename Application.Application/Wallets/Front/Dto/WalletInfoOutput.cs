using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wallets.Front.Dto
{
    public class WalletInfoOutput
    {
        public decimal Withdrawed { get; set; }

        public decimal Withdrawing { get; set; }

        public decimal TotalIncome { get; set; }

        public decimal Balance { get; set; }
    }
}
