using FabricPlatform.API.Machinery.Domain.Model.Commands;
using FabricPlatform.API.Machinery.Interfaces.Rest.Resources;

namespace FabricPlatform.API.Machinery.Interfaces.Rest.Transform;

public static class RegisterMachineCommandFromResourceAssembler
{
    public static RegisterMachineCommand ToCommandFromResource(RegisterMachineResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "RegisterMachineResource cannot be null when converting to command.");
        return new RegisterMachineCommand(resource.Code, resource.Type, resource.Location, resource.AcquiredDate);
    }
}
