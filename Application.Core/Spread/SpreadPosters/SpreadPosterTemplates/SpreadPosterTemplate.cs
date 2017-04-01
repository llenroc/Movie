using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Spread.SpreadPosters.SpreadPosterTemplates
{
    public enum SpreadPosterTemplateParameterType
    {
        Text,
        Picture
    }

    public class SpreadPosterTemplate:FullAuditedEntity,IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Template { get; set; }

        public virtual List<SpreadPosterTemplateParameter> Parameters { get; set; }

        public bool IsDefault { get; set; }
    }

    public class SpreadPosterTemplateParameter:Entity
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

        public SpreadPosterTemplateParameter()
        {
            FontStyle= new FontStyle();
            Coordinate = new Coordinate();
        }
    }

    [ComplexType]
    public class FontStyle
    {
        public int Size { get; set; }

        public string Color { get; set; }

        public string FontFamly { get; set; }
    }

    [ComplexType]
    public class Coordinate
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int StartX { get; set; }

        public int StartY { get; set; }
    }
}
