using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;

namespace Application.Files
{
    public class InfrastructureFileInfo:AuditedEntity,IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}
