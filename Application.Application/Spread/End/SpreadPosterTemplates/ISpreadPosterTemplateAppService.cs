using Application.Spread.End.SpreadPosterTemplates.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Spread.End.SpreadPosterTemplates
{
    public interface ISpreadPosterTemplateAppService : ICrudAppService<SpreadPosterTemplateDto>
    {
        CreateOrEditSpreadPosterTemplateDto GetSpreadPosterTemplateForCreateOrEdit(NullableIdDto input);

        CreateOrEditSpreadPosterTemplateDto CreateOrEdit(CreateOrEditSpreadPosterTemplateDto input);

        void SetAsDefault(SpreadPosterGetInput input);
    }
}
