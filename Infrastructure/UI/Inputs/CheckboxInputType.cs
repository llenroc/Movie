using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Runtime.Validation;

namespace Infrastructure.UI.Inputs
{
    [Serializable]
    [InputType("CHECKBOX")]
    public class CheckboxInputType : InputTypeBase
    {
        public CheckboxInputType(): this(new BooleanValueValidator())
        {

        }

        public CheckboxInputType(IValueValidator validator): base(validator)
        {


        }
    }
}