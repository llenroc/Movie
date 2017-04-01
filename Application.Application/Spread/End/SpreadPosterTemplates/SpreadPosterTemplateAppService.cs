using Application.Spread.SpreadPosters.SpreadPosterTemplates;
using Application.Spread.End.SpreadPosterTemplates.Dto;
using Infrastructure.Application.DTO;
using Infrastructure.Application.Services;
using Infrastructure.AutoMapper;
using Infrastructure.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Application.Spread.End.SpreadPosterTemplates
{
    public class SpreadPosterTemplateAppService : CrudAppService<SpreadPosterTemplate, SpreadPosterTemplateDto>, ISpreadPosterTemplateAppService
    {
        private IRepository<SpreadPosterTemplateParameter> _spreadPosterTemplateParameterRespository;
        public SpreadPosterTemplateAppService(
            IRepository<SpreadPosterTemplateParameter> spreadPosterTemplateParameterRespository,
            IRepository<SpreadPosterTemplate> respository):base(respository)
        {
            _spreadPosterTemplateParameterRespository = spreadPosterTemplateParameterRespository;
        }

        public CreateOrEditSpreadPosterTemplateDto GetSpreadPosterTemplateForCreateOrEdit(NullableIdDto input)
        {
            CreateOrEditSpreadPosterTemplateDto spreadPosterTemplate = new CreateOrEditSpreadPosterTemplateDto();

            if (input.Id.HasValue)
            {
                spreadPosterTemplate = Repository.Get(input.Id.Value).MapTo<CreateOrEditSpreadPosterTemplateDto>();
            }
            return spreadPosterTemplate;
        }

        public void SetAsDefault(SpreadPosterGetInput input)
        {
            SpreadPosterTemplate currentDefault=Repository.GetAll().Where(model => model.IsDefault).FirstOrDefault();

            if (currentDefault != null)
            {
                currentDefault.IsDefault = false;
                Repository.Update(currentDefault);
            }
            SpreadPosterTemplate spreadPosterTemplate = Repository.Get(input.Id);
            spreadPosterTemplate.IsDefault = true;
            Repository.Update(spreadPosterTemplate);
        }

        public CreateOrEditSpreadPosterTemplateDto CreateOrEdit(CreateOrEditSpreadPosterTemplateDto input)
        {
            if (input.Id.HasValue)
            {
                CheckUpdatePermission();

                var entity = GetEntityById(input.Id.Value);
                ObjectMapper.Map(input, entity);

                List<SpreadPosterTemplateParameter> spreadPosterTemplateParameters = new List<SpreadPosterTemplateParameter>();

                foreach (SpreadPosterTemplateParameterDto spreadPosterTemplateParameterDto in input.Parameters)
                {
                    if (spreadPosterTemplateParameterDto.Id.HasValue)
                    {
                        var spreadPosterTemplateParameterInDb = _spreadPosterTemplateParameterRespository.Get(spreadPosterTemplateParameterDto.Id.Value);
                        ObjectMapper.Map(spreadPosterTemplateParameterDto, spreadPosterTemplateParameterInDb);
                        spreadPosterTemplateParameters.Add(spreadPosterTemplateParameterInDb);
                    }
                    else
                    {
                        spreadPosterTemplateParameters.Add(
                            spreadPosterTemplateParameterDto.MapTo<SpreadPosterTemplateParameter>()
                            );
                    }
                }
                entity.Parameters = spreadPosterTemplateParameters;
                CurrentUnitOfWork.SaveChanges();

                return entity.MapTo<CreateOrEditSpreadPosterTemplateDto>();
            }
            else
            {
                CheckCreatePermission();
                var entity = input.MapTo<SpreadPosterTemplate>();

                Repository.Insert(entity);
                CurrentUnitOfWork.SaveChanges();

                return entity.MapTo<CreateOrEditSpreadPosterTemplateDto>();
            }
        }
    }
}
