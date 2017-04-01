using Application.Spread.SpreadPosters.SpreadPosterTemplates;
using Infrastructure.Application.DTO;
using Infrastructure.AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Spread.End.SpreadPosterTemplates.Dto
{
    [AutoMap(typeof(SpreadPosterTemplate))]
    public class SpreadPosterTemplateDto:AuditedEntityDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Template { get; set; }

        public List<SpreadPosterTemplateParameterDto> Parameters { get; set; }

        public bool IsDefault { get; set; }
    }

    [AutoMap(typeof(SpreadPosterTemplateParameter))]
    public class SpreadPosterTemplateParameterDto:NullableIdDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public SpreadPosterTemplateParameterType Type { get; set; }

        public string Value { get; set; }

        public FontStyle FontStyle { get; set; }

        public Coordinate Coordinate { get; set; }

        public SpreadPosterTemplateParameterDto()
        {
            FontStyle = new FontStyle();
            Coordinate = new Coordinate();
        }
    }
}
